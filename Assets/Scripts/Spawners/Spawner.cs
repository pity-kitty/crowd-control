using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using Extensions;
using Player;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Spawners
{
    public abstract class Spawner : MonoBehaviour
    {
        [SerializeField] private SpawnPositions[] spawnPositionsArray;
        [SerializeField] private Body bodyPrefab;
        [SerializeField] private CanvasGroup counterCanvasGroup;
        [SerializeField] private TMP_Text crowdCountText;
        [SerializeField] private int defaultPoolCapacity = 100;
        [SerializeField] private int maxPoolCapacity = 300;
        [SerializeField] protected float moveDuration = 0.5f;

        private ObjectPool<Body> pool;
        protected Dictionary<Guid, Body> bodies = new();
        protected List<PointPosition> freePositions = new();

        public bool CanRegroup { get; protected set; } = true;

        private EventService eventService;

        [Inject]
        private void Construct(EventService eventServiceReference)
        {
            eventService = eventServiceReference;
        }

        private int amountToSpawn;
        private float minRadius;
        private float maxRadius;

        public List<Body> Bodies => bodies.Values.ToList();
        public int BodiesCount => bodies.Count;
        protected EventService EventServiceReference => eventService;

        protected virtual void Start()
        {
            pool = new ObjectPool<Body>(CreateBody, GetBody, ReleaseBody, DestroyBody, 
                false, defaultPoolCapacity, maxPoolCapacity);
            FillFreePositions();
        }

        private Body CreateBody()
        {
            var parentTransform = transform;
            var spawnedBody = Instantiate(bodyPrefab, parentTransform.position, parentTransform.rotation, parentTransform);
            spawnedBody.name = bodyPrefab.name;
            InitialBodySetup(spawnedBody);
            return spawnedBody;
        }

        private void GetBody(Body body)
        {
            body.gameObject.SetActive(true);
            body.InitializeSubscriptions();
        }

        private void ReleaseBody(Body body)
        {
            body.StopBody();
            body.gameObject.SetActive(false);
        }

        private void DestroyBody(Body body)
        {
            body.StopBody();
            Destroy(body.gameObject);
        }

        private void InitialBodySetup(Body spawnedBody)
        {
            spawnedBody.Initialize(eventService, this, RemoveBodyFromList);
        }

        private void FillFreePositions()
        {
            freePositions.Add(new PointPosition { Position = Vector3.zero, Distance = 0 });
            for (var i = 0; i < spawnPositionsArray.Length; i++)
            {
                foreach (var position in spawnPositionsArray[i].Positions)
                {
                    freePositions.Add(new PointPosition { Position = position, Distance = i + 1 });
                }
            }
        }

        protected void SpawnBodies(int spawnCount, bool needAnimate)
        {
            amountToSpawn = spawnCount;
            SpawnRoutine(needAnimate);
        }

        private async void SpawnRoutine(bool needAnimate)
        {
            await Task.Yield();
            freePositions.Sort((a, b) => a.Distance.CompareTo(b.Distance));
            var spawnedCount = 0;
            foreach (var point in freePositions)
            {
                var spawnedBody = pool.Get();
                bodies.Add(spawnedBody.ID, spawnedBody);
                spawnedBody.transform.localPosition = Vector3.zero;
                spawnedBody.transform.DOLocalMove(point.Position, moveDuration);
                spawnedBody.PointPosition = point;
                if (needAnimate) spawnedBody.AnimationToApply = PlayerAnimation.Running;
                else spawnedBody.AnimationToApply = PlayerAnimation.Idle;
                spawnedCount++;
                if (spawnedCount == amountToSpawn) break;
            }

            freePositions.RemoveRange(0, spawnedCount);
            crowdCountText.text = bodies.Count.ToString();
            ShowCounter(true);
        }

        protected virtual void RemoveBodyFromList(Body body)
        {
            pool.Release(body);
            bodies.Remove(body.ID);
            freePositions.Add(body.PointPosition);
            crowdCountText.text = bodies.Count.ToString();
        }

        protected void ShowCounter(bool state)
        {
            counterCanvasGroup.ShowCanvasGroup(state);
        }

        public void DestroyAllBodies()
        {
            foreach (var body in bodies.Values)
            {
                pool.Release(body);
                freePositions.Add(body.PointPosition);
            }

            bodies.Clear();
            crowdCountText.text = bodies.Count.ToString();
        }

        public void SetAnimationForAllBodies(PlayerAnimation playerAnimation)
        {
            foreach (var body in bodies.Values)
            {
                body.SetAnimation(playerAnimation);
            }
        }

        public abstract void RegroupBodies(bool needWait = true);
    }
}
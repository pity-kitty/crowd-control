using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        private ObjectPool<Body> pool;
        private Dictionary<Guid, Body> bodies = new();
        
        private EventService eventService;

        [Inject]
        protected void Construct(EventService eventServiceReference)
        {
            eventService = eventServiceReference;
        }
        
        private int amountToSpawn;
        private float minRadius;
        private float maxRadius;

        [HideInInspector]
        public int BodiesDieLimit = 0;
        public int BodiesCount => bodies.Count;
        public Vector3 SpawnerPosition => transform.position;
        protected EventService EventServiceReference => eventService;
        
        protected virtual void Start()
        {
            pool = new ObjectPool<Body>(CreateBody, GetBody, ReleaseBody, DestroyBody, false, defaultPoolCapacity, maxPoolCapacity);
        }

        private Body CreateBody()
        {
            var parentTransform = transform;
            var spawnedBody = Instantiate(bodyPrefab, parentTransform.position, parentTransform.rotation, parentTransform);
            InitialBodySetup(spawnedBody);
            return spawnedBody;
        }

        private void GetBody(Body body)
        {
            body.gameObject.SetActive(true);
            if (!bodies.TryGetValue(body.ID, out _)) InitialBodySetup(body);
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
            bodies.Add(spawnedBody.ID, spawnedBody);
            spawnedBody.Initialize(eventService, RemoveBodyFromList);
            spawnedBody.MoveConstantly();
        }

        protected void SpawnBodies(int spawnCount, bool needAnimate)
        {
            amountToSpawn = spawnCount;
            SpawnRoutine(needAnimate);
        }

        private async void SpawnRoutine(bool needAnimate)
        {
            await Task.Yield();
            var circlesCount = GetCirclesCount();
            var spawnedBody = pool.Get();
            if (needAnimate) spawnedBody.SetRunningAnimation();
            var spawnedCount = 1;
            for (var i = 0; i < circlesCount; i++)
            {
                var spawnPositions = spawnPositionsArray[i];
                var positions = spawnPositions.Positions;
                foreach (var position in positions)
                {
                    spawnedBody = pool.Get();
                    spawnedBody.transform.localPosition = position;
                    if (needAnimate) spawnedBody.SetRunningAnimation();
                    spawnedCount++;
                    if (spawnedCount == amountToSpawn) break;
                    await Task.Yield();
                }
            }

            crowdCountText.text = bodies.Count.ToString();
            ShowCounter(true);
        }

        private int GetCirclesCount()
        {
            if (amountToSpawn <= 1) return 0;
            var overallLength = 0;
            for (var i = 0; i < spawnPositionsArray.Length; i++)
            {
                overallLength += spawnPositionsArray[i].Positions.Length;
                if (amountToSpawn < overallLength) return ++i;
            }

            return spawnPositionsArray.Length;
        }

        protected virtual void RemoveBodyFromList(Body body)
        {
            pool.Release(body);
            bodies.Remove(body.ID);
            crowdCountText.text = bodies.Count.ToString();
        }

        protected void RestrictDeath()
        {
            foreach (var body in bodies.Values)
            {
                body.CanDie = false;
            }
        }

        protected void ShowCounter(bool state)
        {
            counterCanvasGroup.ShowCanvasGroup(state);
        }

        public void RestrictKill()
        {
            foreach (var body in bodies.Values)
            {
                body.CanKill = false;
            }
        }

        public void DestroyAllBodies()
        {
            foreach (var body in bodies.Values)
            {
                Destroy(body.gameObject);
            }
        }

        public void DestroyRest()
        {
            var countToDestroy = BodiesCount - BodiesDieLimit;
            var destroyedCount = 0;
            foreach (var body in bodies.Values)
            {
                if (destroyedCount <= countToDestroy) break;
                Destroy(body.gameObject);
                destroyedCount++;
            }
        }

        public void SetAnimationForAllBodies(PlayerAnimation playerAnimation)
        {
            foreach (var body in bodies.Values)
            {
                body.SetAnimation(playerAnimation);
            }
        }
    }
}
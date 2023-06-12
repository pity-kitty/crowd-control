using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Extensions;
using Player;
using Services;
using TMPro;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Spawners
{
    public abstract class Spawner : MonoBehaviour
    {
        private const float PlayerSize = 0.7f;
        
        [SerializeField] private Body bodyPrefab;
        [SerializeField] private CanvasGroup counterCanvasGroup;
        [SerializeField] private TMP_Text crowdCountText;

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
        protected EventService EventServiceReference => eventService;
        protected float MaxRadius => maxRadius;

        private Vector3 RandomCircle(Vector3 center)
        {
            var angle = Random.value * 360;
            var radius = Random.Range(minRadius, maxRadius);
            Vector3 spawnPosition;
            spawnPosition.x = center.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            spawnPosition.y = center.y;
            spawnPosition.z = center.z + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            return spawnPosition;
        }

        protected virtual void CalculateRadii()
        {
            minRadius = CalculateCrowdRadius(bodies.Count);
            maxRadius = minRadius + CalculateCrowdRadius(amountToSpawn);
        }

        private float CalculateCrowdRadius(int bodiesCount)
        {
            return Mathf.Sqrt(PlayerSize * bodiesCount / Mathf.PI);
        }

        public void ForceAll()
        {
            foreach (var body in bodies.Values)
            {
                var direction = transform.position - body.Rigidbody.position;
                body.Rigidbody.AddForce(direction, ForceMode.Impulse);
            }
        }

        public void ForceAll(Vector3 position)
        {
            foreach (var body in bodies.Values)
            {
                var direction = position - body.Rigidbody.position;
                body.Rigidbody.AddForce(direction);
                body.SetRunningAnimation();
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
            var spawnerTransform = transform;
            var center = spawnerTransform.position;
            CalculateRadii();
            for (var i = 0; i < amountToSpawn; i++)
            {
                var spawnPosition = RandomCircle(center);
                var spawnedBody = Instantiate(bodyPrefab, spawnPosition, spawnerTransform.rotation, spawnerTransform);
                bodies.Add(spawnedBody.ID, spawnedBody);
                if (needAnimate) spawnedBody.SetRunningAnimation();
                spawnedBody.BodyDestroyed += RemoveBodyFromList;
                await Task.Delay(1);
            }
            crowdCountText.text = bodies.Count.ToString();
            await Task.Yield();
            ForceAll();
        }

        protected virtual void RemoveBodyFromList(Guid guid)
        {
            bodies.Remove(guid);
            crowdCountText.text = bodies.Count.ToString();
        }

        protected void RestrictDeath()
        {
            foreach (var body in bodies.Values)
            {
                body.CanDie = false;
            }
        }

        protected void HideCounter()
        {
            counterCanvasGroup.ShowCanvasGroup(false);
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
                Destroy(body.gameObject);
                destroyedCount++;
                if (destroyedCount == countToDestroy) break;
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
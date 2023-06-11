using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enemy;
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
        private const float PlayerSize = 1f;
        
        [SerializeField] private Body bodyPrefab;
        [SerializeField] private TMP_Text crowdCountText;
        [SerializeField] private Fight fight;


        private Dictionary<Guid, Rigidbody> rigidbodies = new();
        
        private EventService eventService;

        [Inject]
        protected void Construct(EventService eventServiceReference)
        {
            eventService = eventServiceReference;
        }

        protected EventService EventServiceReference => eventService;
        protected Fight Fight => fight;
        protected int BodiesCount => rigidbodies.Count;
        
        private int amountToSpawn;
        private float minRadius;
        private float maxRadius;

        private Vector3 RandomCircle(Vector3 center)
        {
            var angle = Random.value * 360;
            minRadius = CalculateCrowdRadius(rigidbodies.Count);
            maxRadius = minRadius + CalculateCrowdRadius(amountToSpawn);
            var radius = Random.Range(minRadius, maxRadius);
            Vector3 spawnPosition;
            spawnPosition.x = center.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            spawnPosition.y = center.y;
            spawnPosition.z = center.z + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            return spawnPosition;
        }

        private float CalculateCrowdRadius(int bodiesCount)
        {
            return Mathf.Sqrt(PlayerSize * bodiesCount / Mathf.PI);
        }

        private void ForceAll()
        {
            foreach (var rigidbody in rigidbodies.Values)
            {
                var direction = transform.position - rigidbody.position;
                rigidbody.AddForce(direction * 1, ForceMode.Impulse);
            }
        }

        public void ForceAll(Vector3 position)
        {
            foreach (var rigidbody in rigidbodies.Values)
            {
                var direction = position - rigidbody.position;
                rigidbody.AddForce(direction * 1);
            }
        }

        protected void SpawnBodies(int spawnCount)
        {
            amountToSpawn = spawnCount;
            SpawnRoutine();
        }

        private async void SpawnRoutine()
        {
            await Task.Yield();
            var spawnerTransform = transform;
            var center = spawnerTransform.position;
            for (var i = 0; i < amountToSpawn; i++)
            {
                var spawnPosition = RandomCircle(center);
                var spawnedBody = Instantiate(bodyPrefab, spawnPosition, spawnerTransform.rotation, spawnerTransform);
                rigidbodies.Add(spawnedBody.ID, spawnedBody.Rigidbody);
                spawnedBody.BodyDestroyed += RemoveBodyFromList;
                await Task.Delay(1);
            }
            crowdCountText.text = rigidbodies.Count.ToString();
            await Task.Yield();
            ForceAll();
        }

        protected virtual void RemoveBodyFromList(Guid guid)
        {
            rigidbodies.Remove(guid);
            crowdCountText.text = rigidbodies.Count.ToString();
        }
    }
}
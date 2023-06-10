using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Player;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Spawners
{
    public abstract class Spawner : MonoBehaviour
    {
        private const float PlayerSize = 1f;
        
        [SerializeField] private Body bodyPrefab;
        [SerializeField] private TMP_Text crowdCountText;

        protected Dictionary<Guid, Rigidbody> rigidbodies = new();
        
        private int amountToSpawn;
        private float minRadius;
        private float maxRadius;
        
        protected virtual void Start()
        {
            SpawnBodies(1);
        }

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
            foreach (var cubeRigidbody in rigidbodies.Values)
            {
                var direction = transform.position - cubeRigidbody.position;
                cubeRigidbody.AddForce(direction, ForceMode.Impulse);
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
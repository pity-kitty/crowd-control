using System;
using UnityEngine;

namespace Spawners
{
    public class EnemySpawner : Spawner
    {
        [SerializeField] private int spawnCount = 40;

        protected override void Start()
        {
            SpawnBodies(spawnCount);
        }

        protected override void RemoveBodyFromList(Guid guid)
        {
            base.RemoveBodyFromList(guid);
            //TODO: if count is 0 end the game
        }
    }
}
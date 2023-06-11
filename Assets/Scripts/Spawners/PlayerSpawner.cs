using System;
using Services;
using UnityEngine;
using Zenject;

namespace Spawners
{
    public class PlayerSpawner : Spawner
    {
        [Inject] private UserDataService userDataService;

        public Vector3 SpawnerPosition => transform.position;

        private void Start()
        {
            EventServiceReference.GateCollected += SpawnBodies;
        }

        public void SpawnInitialCrowd()
        {
            SpawnBodies(userDataService.CurrentUser.StartCrowdCount);
        }

        protected override void RemoveBodyFromList(Guid guid)
        {
            base.RemoveBodyFromList(guid);
            if (BodiesCount == 0)
            {
                HideCounter();
                EventServiceReference.InvokePlayerLost();
            }
            if (BodiesCount == BodiesDieLimit) EventServiceReference.InvokePlayerDeathLimitReached();
        }

        private void OnDestroy()
        {
            EventServiceReference.GateCollected -= SpawnBodies;
        }
    }
}
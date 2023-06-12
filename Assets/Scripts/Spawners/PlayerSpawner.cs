using System;
using Gate;
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
            EventServiceReference.GateCollected += GateCollected;
        }

        public void SpawnInitialCrowd()
        {
            DestroyAllBodies();
            ShowCounter(true);
            SpawnBodies(userDataService.CurrentUser.StartCrowdCount, false);
        }

        private void GateCollected(MultiplyType multiplyType, int gateValue, bool needAnimate)
        {
            var spawnValue = 0;
            switch (multiplyType)
            {
                case MultiplyType.Addition:
                    spawnValue = gateValue;
                    break;
                case MultiplyType.Multiplication:
                    spawnValue = BodiesCount * gateValue - BodiesCount;
                    break;
            }
            SpawnBodies(spawnValue, needAnimate);
        }

        protected override void CalculateRadii()
        {
            base.CalculateRadii();
            EventServiceReference.InvokeRadiusRecalculated(MaxRadius);
        }

        protected override void RemoveBodyFromList(Guid guid)
        {
            base.RemoveBodyFromList(guid);
            if (BodiesCount == 0)
            {
                ShowCounter(false);
                EventServiceReference.InvokePlayerLost();
            }
            if (BodiesCount == BodiesDieLimit) EventServiceReference.InvokePlayerDeathLimitReached();
        }

        private void OnDestroy()
        {
            EventServiceReference.GateCollected -= GateCollected;
        }
    }
}
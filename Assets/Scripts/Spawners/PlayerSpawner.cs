using System;
using Services;
using UnityEngine;
using Zenject;

namespace Spawners
{
    public class PlayerSpawner : Spawner
    {
        [Inject] private UserDataService userDataService;

        private void Start()
        {
            EventServiceReference.GateCollected += SpawnBodies;
            EventServiceReference.PointOfContactGained += ReceivePointOfContact;
        }

        public void SpawnInitialCrowd()
        {
            SpawnBodies(userDataService.CurrentUser.StartCrowdCount);
        }

        protected override void RemoveBodyFromList(Guid guid)
        {
            base.RemoveBodyFromList(guid);
            if (BodiesCount == 0) EventServiceReference.InvokeFightFinished(false);
        }

        private void ReceivePointOfContact(Vector3 point)
        {
            Fight.StartMoveToPoint(point);
        }

        private void OnDestroy()
        {
            EventServiceReference.GateCollected -= SpawnBodies;
            EventServiceReference.PointOfContactGained -= ReceivePointOfContact;
        }
    }
}
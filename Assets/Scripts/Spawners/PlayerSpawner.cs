using Gate;
using Player;
using Services;
using Zenject;

namespace Spawners
{
    public class PlayerSpawner : Spawner
    {
        [Inject] private UserDataService userDataService;

        protected override void Start()
        {
            base.Start();
            EventServiceReference.GateCollected += GateCollected;
        }

        public void SpawnInitialCrowd()
        {
            DestroyAllBodies();
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

        protected override void RemoveBodyFromList(Body body)
        {
            base.RemoveBodyFromList(body);
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
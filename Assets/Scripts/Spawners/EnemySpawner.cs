using Enemy;
using Player;
using UnityEngine;
using Zenject;

namespace Spawners
{
    public class EnemySpawner : Spawner
    {
        [SerializeField] private int spawnCount = 40;
        [SerializeField] private int playerLayer;
        [SerializeField] private Collider triggerCollider;

        private bool contacted;

        [Inject] private PlayerSpawner playerSpawner;

        protected override void Start()
        {
            base.Start();
            SpawnBodies(spawnCount, false);
        }

        protected override void RemoveBodyFromList(Body body)
        {
            base.RemoveBodyFromList(body);
            if (BodiesCount == 0)
            {
                ShowCounter(false);
                EventServiceReference.InvokeFightFinished();
            }
        }

        public override void RegroupBodies(bool needWait = true) { }

        private void OnTriggerEnter(Collider other)
        {
            if (contacted || other.gameObject.layer != playerLayer) return;
            contacted = true;
            triggerCollider.enabled = false;
            var fight = gameObject.AddComponent(typeof(Fight)) as Fight;
            fight.SetReferences(EventServiceReference, playerSpawner, this);
            fight.StartFight();
        }
    }
}
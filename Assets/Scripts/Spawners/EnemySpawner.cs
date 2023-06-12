using System;
using Enemy;
using UnityEngine;
using Zenject;

namespace Spawners
{
    public class EnemySpawner : Spawner
    {
        [SerializeField] private int spawnCount = 40;
        [SerializeField] private int playerLayer;
        [SerializeField] private Collider triggerCollider;
        [SerializeField] private Rigidbody crowdRigidbody;

        private bool contacted;

        [Inject] private PlayerSpawner playerSpawner;

        private void Start()
        {
            SpawnBodies(spawnCount, false);
        }

        protected override void RemoveBodyFromList(Guid guid)
        {
            base.RemoveBodyFromList(guid);
            if (BodiesCount == 0) HideCounter();
            if (BodiesCount == BodiesDieLimit) RestrictDeath();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (contacted || other.gameObject.layer != playerLayer) return;
            contacted = true;
            triggerCollider.enabled = false;
            crowdRigidbody.velocity = Vector3.zero;
            crowdRigidbody.angularVelocity = Vector3.zero;
            var fight = gameObject.AddComponent(typeof(Fight)) as Fight;
            fight.SetReferences(EventServiceReference, playerSpawner, this);
            fight.StartFight();
        }
    }
}
using System;
using UnityEngine;

namespace Spawners
{
    public class EnemySpawner : Spawner
    {
        [SerializeField] private int spawnCount = 40;
        [SerializeField] private int playerLayer;
        [SerializeField] private Collider triggerCollider;
        [SerializeField] private Rigidbody crowdRigidbody;

        private bool contacted;

        private void Start()
        {
            SpawnBodies(spawnCount);
        }

        protected override void RemoveBodyFromList(Guid guid)
        {
            base.RemoveBodyFromList(guid);
            if (BodiesCount == 0) EventServiceReference.InvokeFightFinished(true);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (contacted || other.gameObject.layer != playerLayer) return;
            contacted = true;
            triggerCollider.enabled = false;
            crowdRigidbody.velocity = Vector3.zero;
            crowdRigidbody.angularVelocity = Vector3.zero; 
            var pointOfContact = other.contacts[0].point;
            pointOfContact.y = 0;
            pointOfContact.z -= 2f;
            Fight.StartMoveToPoint(pointOfContact);
            EventServiceReference.InvokePointOfContactGained(transform.position);
            EventServiceReference.InvokeFightStarted();
        }
    }
}
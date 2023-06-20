using UnityEngine;

namespace Player
{
    public class PlayerBody : Body
    {
        [SerializeField] private int obstacleLayer;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != obstacleLayer) return;
            KillBody();
            if (Spawner.CanRegroup) Spawner.RegroupBodies();
        }
    }
}
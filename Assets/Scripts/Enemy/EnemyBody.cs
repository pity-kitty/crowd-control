using Player;
using UnityEngine;

namespace Enemy
{
    public class EnemyBody : Body
    {
        [SerializeField] private int playerLayer;
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer != playerLayer) return;
            if (CanKill) Destroy(other.gameObject);
            if (CanDie) Destroy(gameObject);
        }
    }
}
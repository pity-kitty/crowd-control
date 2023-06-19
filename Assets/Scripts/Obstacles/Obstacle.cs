using UnityEngine;

namespace Obstacles
{
    public class Obstacle : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            //Destroy(other.gameObject);
        }
    }
}
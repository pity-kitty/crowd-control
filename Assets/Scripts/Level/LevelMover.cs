using UnityEngine;

namespace Level
{
    public class LevelMover : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 10f;

        private void Update()
        {
            var moveDirection = -Vector3.forward * (moveSpeed * Time.deltaTime);
            transform.Translate(moveDirection);
        }
    }
}
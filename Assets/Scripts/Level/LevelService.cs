using System.Collections;
using UnityEngine;

namespace Level
{
    public class LevelService : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 10f;

        private Coroutine moveRoutine;

        private IEnumerator MoveLevel()
        {
            while (true)
            {
                var moveDirection = -Vector3.forward * (moveSpeed * Time.deltaTime);
                transform.Translate(moveDirection);
                yield return null;
            }
        }

        public void StartLevelMovement()
        {
            moveRoutine = StartCoroutine(MoveLevel());
        }

        public void StopMovement()
        {
            StopCoroutine(moveRoutine);
        }

        public void ResetLevel()
        {
            transform.position = Vector3.zero;
        }
    }
}
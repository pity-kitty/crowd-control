using System.Collections;
using Services;
using Spawners;
using UnityEngine;
using Zenject;

namespace Enemy
{
    public class Fight : MonoBehaviour
    {
        [SerializeField] private Spawner bodySpawner;

        private Coroutine moveRoutine;
        
        [Inject] private EventService eventService;
        
        private Vector3 pointToMove;

        public void StartMoveToPoint(Vector3 point)
        {
            pointToMove = point;
            moveRoutine = StartCoroutine(MoveToPoint());
            eventService.FightFinished += StopMovement;
        }

        private IEnumerator MoveToPoint()
        {
            while (true)
            {
                bodySpawner.ForceAll(pointToMove);
                yield return new WaitForFixedUpdate();
            }
        }

        private void StopMovement(bool hasPlayerWon)
        {
            eventService.FightFinished -= StopMovement;
            StopCoroutine(moveRoutine);
        }
    }
}
using System.Collections;
using Player;
using Services;
using Spawners;
using UnityEngine;

namespace Enemy
{
    public class Fight : MonoBehaviour
    {
        private const float TimeToWait = 3f;

        private int income;

        private EventService eventService;
        private PlayerSpawner playerSpawner;
        private EnemySpawner enemySpawner;
        private Coroutine playerMoveCoroutine;
        private Coroutine enemyMoveCoroutine;

        public void SetReferences(EventService eventService, PlayerSpawner playerSpawner, EnemySpawner enemySpawner)
        {
            this.eventService = eventService;
            this.playerSpawner = playerSpawner;
            this.enemySpawner = enemySpawner;
        }

        public void StartFight()
        {
            eventService.InvokeFightStarted();
            enemySpawner.SetAnimationForAllBodies(PlayerAnimation.Running);
            var playerBodies = playerSpawner.BodiesCount;
            var enemyBodies = enemySpawner.BodiesCount;
            var dieLimit = playerBodies - enemyBodies;
            if (dieLimit > 0)
            {
                playerSpawner.BodiesDieLimit = dieLimit;
                income = enemyBodies;
            }
            else
            {
                enemySpawner.BodiesDieLimit = -dieLimit;
                income = playerBodies;
            }
            eventService.PlayerDeathLimitReached += enemySpawner.RestrictKill;
            enemyMoveCoroutine = StartCoroutine(MoveToPoint(enemySpawner, playerSpawner.SpawnerPosition));
            playerMoveCoroutine = StartCoroutine(MoveToPoint(playerSpawner, enemySpawner.SpawnerPosition));
            StartCoroutine(DetermineFightResult());
        }

        private IEnumerator MoveToPoint(Spawner spawner, Vector3 point)
        {
            while (true)
            {
                spawner.ForceAll(point);
                yield return new WaitForFixedUpdate();
            }
        }

        private IEnumerator DetermineFightResult()
        {
            yield return new WaitForSeconds(TimeToWait);
            StopCoroutine(playerMoveCoroutine);
            StopCoroutine(enemyMoveCoroutine);
            if (enemySpawner.BodiesDieLimit == 0) enemySpawner.DestroyAllBodies();
            eventService.InvokeMoneyGained(income);
            if (playerSpawner.BodiesDieLimit == 0)
            {
                playerSpawner.DestroyAllBodies();
                enemySpawner.DestroyRest();
                eventService.InvokeFightFinished(false);
            }
            else
            {
                playerSpawner.DestroyRest();
                yield return new WaitForFixedUpdate();
                playerSpawner.BodiesDieLimit = 0;
                playerSpawner.ForceAll();
                eventService.InvokeFightFinished(true);
            }

            eventService.PlayerDeathLimitReached -= enemySpawner.RestrictKill;
        }
    }
}
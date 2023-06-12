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

        private Vector3 pointToMove;
        private int income;

        private EventService eventService;
        private PlayerSpawner playerSpawner;
        private EnemySpawner enemySpawner;

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
            pointToMove = playerSpawner.SpawnerPosition;
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
            StartCoroutine(MoveToPoint());
        }

        private IEnumerator MoveToPoint()
        {
            var finishTime = Time.time + TimeToWait;
            while (finishTime > Time.time)
            {
                enemySpawner.ForceAll(pointToMove);
                yield return new WaitForFixedUpdate();
            }

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
                playerSpawner.BodiesDieLimit = 0;
                yield return new WaitForFixedUpdate();
                playerSpawner.ForceAll();
                eventService.InvokeFightFinished(true);
            }

            eventService.PlayerDeathLimitReached -= enemySpawner.RestrictKill;
        }
    }
}
using System.Collections;
using System.Threading.Tasks;
using Services;
using Spawners;
using UnityEngine;

namespace Enemy
{
    public class Fight : MonoBehaviour
    {
        private const float TimeToWait = 3f;

        private Vector3 pointToMove;

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
            var playerBodies = playerSpawner.BodiesCount;
            var enemyBodies = enemySpawner.BodiesCount;
            var dieLimit = playerBodies - enemyBodies;
            if (dieLimit > 0) playerSpawner.BodiesDieLimit = dieLimit;
            else enemySpawner.BodiesDieLimit = -dieLimit;
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
                playerSpawner.ForceAll();
                eventService.InvokeFightFinished(true);
            }

            eventService.PlayerDeathLimitReached -= enemySpawner.RestrictKill;
        }
    }
}
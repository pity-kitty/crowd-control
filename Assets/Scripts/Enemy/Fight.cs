using System.Collections;
using System.Collections.Generic;
using Player;
using Services;
using Spawners;
using UnityEngine;

namespace Enemy
{
    public class Fight : MonoBehaviour
    {
        private int income;

        private EventService eventService;
        private PlayerSpawner playerSpawner;
        private EnemySpawner enemySpawner;
        private Coroutine fightCoroutine;

        public void SetReferences(EventService eventService, PlayerSpawner playerSpawner, EnemySpawner enemySpawner)
        {
            this.eventService = eventService;
            this.playerSpawner = playerSpawner;
            this.enemySpawner = enemySpawner;
        }

        public void StartFight()
        {
            eventService.InvokeFightStarted();
            eventService.FightFinished += OnFightEnd;
            enemySpawner.SetAnimationForAllBodies(PlayerAnimation.Running);
            var playerBodies = playerSpawner.Bodies;
            playerBodies.Sort((a, b) => a.PointPosition.Position.z.CompareTo(b.PointPosition.Position.z));
            playerBodies.Reverse();
            var enemyBodies = enemySpawner.Bodies;
            enemyBodies.Sort((a, b) => a.PointPosition.Position.z.CompareTo(b.PointPosition.Position.z));
            var playerBodiesCount = playerSpawner.BodiesCount;
            var enemyBodiesCount = enemySpawner.BodiesCount;
            var dieLimit = playerBodiesCount - enemyBodiesCount;
            if (dieLimit > 0)
            {
                fightCoroutine = StartCoroutine(InitializeFight(playerBodies, enemyBodies));
                income = enemyBodiesCount;
            }
            else
            {
                fightCoroutine = StartCoroutine(InitializeFight(enemyBodies, playerBodies));
                income = playerBodiesCount;
            }
        }

        private IEnumerator InitializeFight(List<Body> winner, List<Body> loser)
        {
            var loserIndex = 0;
            var canDie = true;
            var loserCount = loser.Count;
            foreach (var body in winner)
            {
                if (loserIndex == loserCount)
                {
                    loserIndex = 0;
                    canDie = false;
                }
                var loserBody = loser[loserIndex];
                var movePosition = Vector3.Lerp(body.transform.position, loserBody.transform.position, 0.5f);
                body.Move(movePosition, canDie);
                if (canDie) loserBody.Move(movePosition, true);
                loserIndex++;
                yield return null;
            }
            eventService.InvokeMoneyGained(income);
        }

        private void OnFightEnd()
        {
            eventService.FightFinished -= OnFightEnd;
            StopCoroutine(fightCoroutine);
            eventService.InvokeMoneyGained(income);
        }
    }
}
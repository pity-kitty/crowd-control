using System.Collections;
using System.Linq;
using DG.Tweening;
using Gate;
using Player;
using Services;
using UnityEngine;
using Zenject;

namespace Spawners
{
    public class PlayerSpawner : Spawner
    {
        [SerializeField] private float regroupDelay = 1.5f;
        
        private UserDataService userDataService;

        [Inject]
        private void Construct(UserDataService userDataServiceReference)
        {
            userDataService = userDataServiceReference;
        }

        protected override void Start()
        {
            base.Start();
            EventServiceReference.FightFinished += () =>
            {
                ReturnBodies();
                RegroupBodies(false);
            };
            EventServiceReference.GateCollected += GateCollected;
        }

        public void SpawnInitialCrowd()
        {
            DestroyAllBodies();
            SpawnBodies(userDataService.CurrentUser.StartCrowdCount, false);
        }

        protected override void SpawnBodies(int spawnCount, bool needAnimate)
        {
            base.SpawnBodies(spawnCount, needAnimate);
            EventServiceReference.InvokeRadiusRecalculated(GetCirclesCount(spawnCount));
        }
        
        private int GetCirclesCount(int spawnCount = 0)
        {
            var count = BodiesCount + spawnCount;
            if (count <= 1) return 0;
            var overallLength = 0;
            for (var i = 0; i < spawnPositionsArray.Length; i++)
            {
                overallLength += spawnPositionsArray[i].Positions.Length;
                if (count < overallLength) return ++i;
            }

            return spawnPositionsArray.Length;
        }

        private void GateCollected(MultiplyType multiplyType, int gateValue, bool needAnimate)
        {
            var spawnValue = 0;
            switch (multiplyType)
            {
                case MultiplyType.Addition:
                    spawnValue = gateValue;
                    break;
                case MultiplyType.Multiplication:
                    spawnValue = BodiesCount * gateValue - BodiesCount;
                    break;
            }
            SpawnBodies(spawnValue, needAnimate);
        }

        public override void RegroupBodies(bool needWait = true)
        {
            if (!CanRegroup) return;
            StartCoroutine(Reposition(needWait));
            CanRegroup = false;
        }

        private IEnumerator Reposition(bool needWait)
        {
            if (needWait) yield return new WaitForSeconds(regroupDelay);
            if (bodies.Count == 0)
            {
                CanRegroup = true;
                yield break;
            }

            var bodiesList = bodies.Values.ToList();
            bodiesList.Sort((a, b) => a.PointPosition.Distance.CompareTo(b.PointPosition.Distance));
            bodiesList.Reverse();
            freePositions.Sort((a, b) => a.Distance.CompareTo(b.Distance));
            var bodiesCount = bodiesList.Count;
            var countToRemove = 0;
            for (var i = 0; i < freePositions.Count; i++)
            {
                if (i == bodiesCount) break;
                var point = freePositions[i];
                var body = bodiesList[i];
                if (point.Distance >= body.PointPosition.Distance)
                {
                    
                    break;
                }
                freePositions.Add(body.PointPosition);
                body.PointPosition = point;
                body.transform.DOLocalMove(point.Position, moveDuration);
                countToRemove++;
            }

            freePositions.RemoveRange(0, countToRemove);
            EventServiceReference.InvokeRadiusRecalculated(GetCirclesCount());
            yield return null;
            CanRegroup = true;
        }

        private void ReturnBodies()
        {
            foreach (var body in bodies.Values)
            {
                body.transform.DOLocalMove(body.PointPosition.Position, moveDuration);
            }
        }

        protected override void RemoveBodyFromList(Body body)
        {
            base.RemoveBodyFromList(body);
            if (BodiesCount == 0)
            {
                ShowCounter(false);
                EventServiceReference.InvokePlayerLost();
            }
        }

        private void OnDestroy()
        {
            EventServiceReference.GateCollected -= GateCollected;
        }
    }
}
using System;
using UnityEngine;

namespace Services
{
    public class EventService
    {
        private Action<int> gateCollected;
        private Action fightStarted;
        private Action<bool> fightFinished;
        private Action<Vector3> pointOfContactGained;
        
        public event Action<int> GateCollected
        {
            add => gateCollected += value;
            remove => gateCollected -= value;
        }
        
        public event Action FightStarted
        {
            add => fightStarted += value;
            remove => fightStarted -= value;
        }
        
        public event Action<bool> FightFinished
        {
            add => fightFinished += value;
            remove => fightFinished -= value;
        }
        
        public event Action<Vector3> PointOfContactGained
        {
            add => pointOfContactGained += value;
            remove => pointOfContactGained -= value;
        }

        public void InvokeGateCollectedEvent(int gateValue)
        {
            gateCollected?.Invoke(gateValue);
        }
        
        public void InvokeFightStarted()
        {
            fightStarted?.Invoke();
        }

        public void InvokeFightFinished(bool hasPlayerWon)
        {
            fightFinished?.Invoke(hasPlayerWon);
        }

        public void InvokePointOfContactGained(Vector3 point)
        {
            pointOfContactGained?.Invoke(point);
        }
    }
}
using System;

namespace Services
{
    public class EventService
    {
        private Action<int> gateCollected;
        private Action fightStarted;
        private Action<bool> fightFinished;
        private Action playerDeathLimitReached;
        private Action playerLost;
        
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
        
        public event Action PlayerDeathLimitReached
        {
            add => playerDeathLimitReached += value;
            remove => playerDeathLimitReached -= value;
        }
        
        public event Action PlayerLost
        {
            add => playerLost += value;
            remove => playerLost -= value;
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

        public void InvokePlayerDeathLimitReached()
        {
            playerDeathLimitReached?.Invoke();
        }

        public void InvokePlayerLost()
        {
            playerLost?.Invoke();
        }
    }
}
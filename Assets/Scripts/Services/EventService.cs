using System;
using Gate;

namespace Services
{
    public class EventService
    {
        private Action<MultiplyType, int, bool> gateCollected;
        private Action fightStarted;
        private Action<bool> fightFinished;
        private Action playerDeathLimitReached;
        private Action playerLost;
        private Action playerWon;
        private Action<float> radiusRecalculated;
        private Action tryAgainPressed;
        private Action nextLevelPressed;
        private Action<int> moneyGained;

        public event Action<MultiplyType, int, bool> GateCollected
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
        
        public event Action PlayerWon
        {
            add => playerWon += value;
            remove => playerWon -= value;
        }
        
        public event Action<float> RadiusRecalculated
        {
            add => radiusRecalculated += value;
            remove => radiusRecalculated -= value;
        }
        
        public event Action TryAgainPressed
        {
            add => tryAgainPressed += value;
            remove => tryAgainPressed -= value;
        }
        
        public event Action NextLevelPressed
        {
            add => nextLevelPressed += value;
            remove => nextLevelPressed -= value;
        }
        
        public event Action<int> MoneyGained
        {
            add => moneyGained += value;
            remove => moneyGained -= value;
        }

        public void InvokeGateCollectedEvent(MultiplyType multiplyType, int gateValue)
        {
            gateCollected?.Invoke(multiplyType, gateValue, true);
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

        public void InvokePlayerWon()
        {
            playerWon?.Invoke();
        }

        public void InvokeRadiusRecalculated(float maxRadius)
        {
            radiusRecalculated?.Invoke(maxRadius);
        }

        public void InvokeTryAgainPressed()
        {
            tryAgainPressed?.Invoke();
        }

        public void InvokeNextLevelPressed()
        {
            nextLevelPressed?.Invoke();
        }

        public void InvokeMoneyGained(int money)
        {
            moneyGained?.Invoke(money);
        }
    }
}
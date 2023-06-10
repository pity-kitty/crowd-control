using System;

namespace Services
{
    public class EventService
    {
        private Action<int> gateCollected;
        
        public event Action<int> GateCollected
        {
            add => gateCollected += value;
            remove => gateCollected -= value;
        }

        public void InvokeGateCollectedEvent(int gateValue)
        {
            gateCollected?.Invoke(gateValue);
        }
    }
}
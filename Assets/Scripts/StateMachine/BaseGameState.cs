using Services;

namespace StateMachine
{
    public abstract class BaseGameState
    {
        protected GameStateContext gameStateContext;
        protected EventService eventService;

        protected BaseGameState(GameStateContext gameStateContext, EventService eventService)
        {
            this.gameStateContext = gameStateContext;
            this.eventService = eventService;
        }
        
        public abstract void EnterState();
        public abstract void LeaveState();
    }
}
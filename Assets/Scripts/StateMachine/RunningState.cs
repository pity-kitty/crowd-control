using Services;

namespace StateMachine
{
    public class RunningState : BaseGameState
    {
        public RunningState(GameStateContext gameStateContext, EventService eventService) : base(gameStateContext, eventService) { }
        
        public override void EnterState()
        {
            gameStateContext.LevelService.StartLevelMovement();
            eventService.FightStarted += LeaveState;
            eventService.PlayerLost += PlayerLost;
        }

        public override void LeaveState()
        {
            eventService.FightStarted -= LeaveState;
            eventService.PlayerLost -= PlayerLost;
            gameStateContext.SwitchState(gameStateContext.FightingState);
        }

        private void PlayerLost()
        {
            eventService.FightStarted -= LeaveState;
            eventService.PlayerLost -= PlayerLost;
            gameStateContext.SwitchState(gameStateContext.MainMenuState);
        }
    }
}
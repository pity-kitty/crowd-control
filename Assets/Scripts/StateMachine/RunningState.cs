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
        }

        public override void LeaveState()
        {
            eventService.FightStarted -= LeaveState;
            gameStateContext.SwitchState(gameStateContext.FightingState);
        }

    }
}
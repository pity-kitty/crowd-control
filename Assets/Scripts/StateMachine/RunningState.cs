using Player;
using Services;

namespace StateMachine
{
    public class RunningState : BaseGameState
    {
        public RunningState(GameStateContext gameStateContext, EventService eventService) : base(gameStateContext, eventService) { }
        
        public override void EnterState()
        {
            gameStateContext.PlayerControl.EnableControl(true);
            gameStateContext.LevelService.StartLevelMovement();
            gameStateContext.PlayerSpawner.SetAnimationForAllBodies(PlayerAnimation.Running);
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
            gameStateContext.EndScreenUI.ShowGameOverScreen(true);
            gameStateContext.SwitchState(gameStateContext.EndState);
        }
    }
}
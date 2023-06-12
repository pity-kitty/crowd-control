using Services;

namespace StateMachine
{
    public class EndState : BaseGameState
    {
        public EndState(GameStateContext gameStateContext, EventService eventService) : base(gameStateContext, eventService) { }

        public override void EnterState()
        {
            gameStateContext.LevelService.StopMovement();
            gameStateContext.PlayerControl.EnableControl(false);
            gameStateContext.EndScreenUI.ShowEndScreen(true);
            eventService.TryAgainPressed += LeaveState;
            eventService.NextLevelPressed += LoadNextLevel;
        }

        public override void LeaveState()
        {
            eventService.TryAgainPressed -= LeaveState;
            eventService.NextLevelPressed -= LoadNextLevel;
            gameStateContext.SwitchState(gameStateContext.MainMenuState);
        }

        private void LoadNextLevel()
        {
            eventService.TryAgainPressed -= LeaveState;
            eventService.NextLevelPressed -= LoadNextLevel;
            gameStateContext.SwitchState(gameStateContext.MainMenuState);
        }
    }
}
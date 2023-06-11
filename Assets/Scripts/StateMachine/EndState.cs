using Level;
using Services;
using UI;

namespace StateMachine
{
    public class EndState : BaseGameState
    {
        private readonly EndScreenUI endScreenUI;
        private readonly LevelService levelService;

        public EndState(GameStateContext gameStateContext, EventService eventService, LevelService levelService, EndScreenUI endScreenUI)
            : base(gameStateContext, eventService)
        {
            this.endScreenUI = endScreenUI;
            this.levelService = levelService;
        }

        public override void EnterState()
        {
            levelService.StopMovement();
            endScreenUI.ShowEndScreen(true);
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
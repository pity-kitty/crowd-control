using Services;

namespace StateMachine
{
    public class MainMenuState : BaseGameState
    {
        public MainMenuState(GameStateContext gameStateContext, EventService eventService) : base(gameStateContext, eventService) { }
        
        public override void EnterState()
        {
            gameStateContext.LevelService.StopMovement();
            gameStateContext.LevelService.ResetLevel();
            gameStateContext.PlayerSpawner.SpawnInitialCrowd();
            gameStateContext.MainUI.OnGameStarted += LeaveState;
        }

        public override void LeaveState()
        {
            gameStateContext.MainUI.OnGameStarted -= LeaveState;
            gameStateContext.SwitchState(gameStateContext.RunningState);
        }
    }
}
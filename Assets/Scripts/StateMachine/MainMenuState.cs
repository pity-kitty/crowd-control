using Level;
using Player;
using Services;
using Spawners;
using UI;

namespace StateMachine
{
    public class MainMenuState : BaseGameState
    {
        private readonly MainUI mainUI;
        private readonly LevelService levelService;
        private readonly PlayerSpawner playerSpawner;
        private readonly PlayerControl playerControl;
        private readonly EndScreenUI endScreenUI;

        public MainMenuState(GameStateContext gameStateContext, EventService eventService, MainUI mainUI,
            LevelService levelService, PlayerSpawner playerSpawner, PlayerControl playerControl, EndScreenUI endScreenUI) 
            : base(gameStateContext, eventService)
        {
            this.mainUI = mainUI;
            this.levelService = levelService;
            this.playerSpawner = playerSpawner;
            this.playerControl = playerControl;
            this.endScreenUI = endScreenUI;
        }
        
        public override void EnterState()
        {
            endScreenUI.ShowEndScreen(false);
            mainUI.ShowUI(true);
            playerControl.EnableControl(false);
            levelService.StopMovement();
            levelService.ResetLevel();
            playerSpawner.SpawnInitialCrowd();
            mainUI.OnGameStarted += LeaveState;
        }

        public override void LeaveState()
        {
            mainUI.OnGameStarted -= LeaveState;
            gameStateContext.SwitchState(gameStateContext.RunningState);
        }
    }
}
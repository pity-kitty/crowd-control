using Level;
using Player;
using Services;
using UI;

namespace StateMachine
{
    public class RunningState : BaseGameState
    {
        private readonly LevelService levelService;
        private readonly PlayerControl playerControl;
        private readonly EndScreenUI endScreenUI;

        public RunningState(GameStateContext gameStateContext, EventService eventService, LevelService levelService, 
            PlayerControl playerControl, EndScreenUI endScreenUI) : base(gameStateContext, eventService)
        {
            this.levelService = levelService;
            this.playerControl = playerControl;
            this.endScreenUI = endScreenUI;
        }
        
        public override void EnterState()
        {
            playerControl.EnableControl(true);
            levelService.StartLevelMovement();
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
            endScreenUI.ShowGameOverScreen(true);
            gameStateContext.SwitchState(gameStateContext.EndState);
        }
    }
}
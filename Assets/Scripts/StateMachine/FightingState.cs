using Level;
using Player;
using Services;
using UI;

namespace StateMachine
{
    public class FightingState : BaseGameState
    {
        private readonly LevelService levelService;
        private readonly PlayerControl playerControl;
        private readonly EndScreenUI endScreenUI;
        private bool isPlayerAlive;

        public FightingState(GameStateContext gameStateContext, EventService eventService, LevelService levelService, 
            PlayerControl playerControl, EndScreenUI endScreenUI) : base(gameStateContext, eventService)
        {
            this.levelService = levelService;
            this.playerControl = playerControl;
            this.endScreenUI = endScreenUI;
        }

        public override void EnterState()
        {
            playerControl.EnableControl(false);
            levelService.StopMovement();
            eventService.FightFinished += OnFightFinish;
        }

        public override void LeaveState()
        {
            eventService.FightFinished -= OnFightFinish;
            if (isPlayerAlive)
            {
                gameStateContext.SwitchState(gameStateContext.RunningState);
                return;
            }
            
            endScreenUI.ShowGameOverScreen(true);
            gameStateContext.SwitchState(gameStateContext.EndState);
        }

        private void OnFightFinish(bool isAlive)
        {
            isPlayerAlive = isAlive;
            LeaveState();
        }
    }
}
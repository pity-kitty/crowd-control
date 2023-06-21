using Services;

namespace StateMachine
{
    public class FightingState : BaseGameState
    {
        private bool isPlayerAlive;
        
        public FightingState(GameStateContext gameStateContext, EventService eventService) : base(gameStateContext, eventService) { }

        public override void EnterState()
        {
            gameStateContext.PlayerControl.EnableControl(false);
            gameStateContext.LevelService.StopMovement();
            eventService.FightFinished += OnFightFinish;
            eventService.PlayerLost += OnPlayerLose;
        }

        public override void LeaveState()
        {
            eventService.FightFinished -= OnFightFinish;
            eventService.PlayerLost -= OnPlayerLose;
            if (isPlayerAlive)
            {
                gameStateContext.SwitchState(gameStateContext.RunningState);
                return;
            }
            
            gameStateContext.Wallet.ShowIncome(true);
            gameStateContext.EndScreenUI.ShowGameOverScreen(true);
            gameStateContext.SwitchState(gameStateContext.EndState);
        }

        private void OnFightFinish()
        {
            isPlayerAlive = true;
            LeaveState();
        }

        private void OnPlayerLose()
        {
            isPlayerAlive = false;
            LeaveState();
        }
    }
}
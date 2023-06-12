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
        }

        public override void LeaveState()
        {
            eventService.FightFinished -= OnFightFinish;
            if (isPlayerAlive)
            {
                gameStateContext.SwitchState(gameStateContext.RunningState);
                return;
            }
            
            gameStateContext.EndScreenUI.ShowGameOverScreen(true);
            gameStateContext.SwitchState(gameStateContext.EndState);
        }

        private void OnFightFinish(bool isAlive)
        {
            isPlayerAlive = isAlive;
            LeaveState();
        }
    }
}
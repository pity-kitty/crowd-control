using Services;

namespace StateMachine
{
    public class FightingState : BaseGameState
    {
        private bool isPlayerAlive;
        
        public FightingState(GameStateContext gameStateContext, EventService eventService) : base(gameStateContext, eventService) { }

        public override void EnterState()
        {
            gameStateContext.LevelService.StopMovement();
        }

        public override void LeaveState()
        {
            eventService.FightFinished -= OnFightFinish;
            if (isPlayerAlive)
            {
                gameStateContext.SwitchState(gameStateContext.RunningState);
                return;
            }
            
            gameStateContext.SwitchState(gameStateContext.MainMenuState);
        }

        private void OnFightFinish(bool isAlive)
        {
            isPlayerAlive = isAlive;
            LeaveState();
        }
    }
}
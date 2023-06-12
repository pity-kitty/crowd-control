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
            gameStateContext.Wallet.ResetIncome();
            eventService.TryAgainPressed += LeaveState;
            eventService.NextLevelPressed += LeaveState;
        }

        public override void LeaveState()
        {
            eventService.TryAgainPressed -= LeaveState;
            eventService.NextLevelPressed -= LeaveState;
            gameStateContext.Wallet.ShowIncome(false);
            gameStateContext.Wallet.SetOverallMoney();
            gameStateContext.SwitchState(gameStateContext.MainMenuState);
        }
    }
}
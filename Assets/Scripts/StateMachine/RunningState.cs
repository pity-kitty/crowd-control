using Player;
using Services;

namespace StateMachine
{
    public class RunningState : BaseGameState
    {
        private readonly UserDataService userDataService;

        public RunningState(GameStateContext gameStateContext, EventService eventService, UserDataService userDataService) 
            : base(gameStateContext, eventService)
        {
            this.userDataService = userDataService;
        }
        
        public override void EnterState()
        {
            gameStateContext.PlayerControl.EnableControl(true);
            gameStateContext.LevelService.StartLevelMovement();
            gameStateContext.PlayerSpawner.SetAnimationForAllBodies(PlayerAnimation.Running);
            eventService.FightStarted += LeaveState;
            eventService.PlayerLost += PlayerLost;
            eventService.PlayerWon += PlayerWon;
        }

        public override void LeaveState()
        {
            eventService.FightStarted -= LeaveState;
            eventService.PlayerLost -= PlayerLost;
            eventService.PlayerWon -= PlayerWon;
            gameStateContext.SwitchState(gameStateContext.FightingState);
        }

        private void PlayerLost()
        {
            eventService.FightStarted -= LeaveState;
            eventService.PlayerLost -= PlayerLost;
            eventService.PlayerWon -= PlayerWon;
            gameStateContext.EndScreenUI.ShowGameOverScreen(true);
            gameStateContext.SwitchState(gameStateContext.EndState);
        }

        private void PlayerWon()
        {
            eventService.FightStarted -= LeaveState;
            eventService.PlayerLost -= PlayerLost;
            eventService.PlayerWon -= PlayerWon;
            gameStateContext.PlayerSpawner.SetAnimationForAllBodies(PlayerAnimation.Cheering);
            gameStateContext.EndScreenUI.ShowVictoryScreen(true);
            userDataService.CurrentUser.Level++;
            userDataService.SaveUserData();
            gameStateContext.SwitchState(gameStateContext.EndState);
        }
    }
}
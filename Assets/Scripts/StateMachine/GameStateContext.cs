using Level;
using Player;
using Services;
using Spawners;
using UI;
using UnityEngine;
using Zenject;

namespace StateMachine
{
    public class GameStateContext : MonoBehaviour
    {
        [SerializeField] private MainUI mainUI;
        [SerializeField] private PlayerSpawner playerSpawner;
        [SerializeField] private LevelService levelService;
        [SerializeField] private PlayerControl playerControl;
        [SerializeField] private EndScreenUI endScreenUI;

        private BaseGameState currentState;

        private EventService eventService;

        [Inject]
        private void Construct(EventService eventServiceReference)
        {
            eventService = eventServiceReference;
        }

        public MainMenuState MainMenuState;
        public RunningState RunningState;
        public FightingState FightingState;
        public EndState EndState;

        private void Awake()
        {
            MainMenuState = new MainMenuState(this, eventService, mainUI, levelService, playerSpawner, playerControl, endScreenUI);
            RunningState = new RunningState(this, eventService, levelService, playerControl, endScreenUI);
            FightingState = new FightingState(this, eventService, levelService, playerControl, endScreenUI);
            EndState = new EndState(this, eventService, levelService, endScreenUI);
        }

        private void Start()
        {
            currentState = MainMenuState;
            currentState.EnterState();
        }

        public void SwitchState(BaseGameState newGameState)
        {
            currentState = newGameState;
            currentState.EnterState();
        }
    }
}
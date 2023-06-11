using Level;
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

        public MainUI MainUI => mainUI;
        public PlayerSpawner PlayerSpawner => playerSpawner;
        public LevelService LevelService => levelService;

        private void Awake()
        {
            MainMenuState = new MainMenuState(this, eventService);
            RunningState = new RunningState(this, eventService);
            FightingState = new FightingState(this, eventService);
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
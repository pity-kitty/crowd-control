using System.Collections;
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
        private const string AnimationTextureFileName = "animationtexture";

        [SerializeField] private MainUI mainUI;
        [SerializeField] private PlayerSpawner playerSpawner;
        [SerializeField] private LevelService levelService;
        [SerializeField] private PlayerControl playerControl;
        [SerializeField] private EndScreenUI endScreenUI;
        [SerializeField] private Wallet wallet;
        
        private BaseGameState currentState;

        public MainUI MainUI => mainUI;
        public PlayerSpawner PlayerSpawner => playerSpawner;
        public LevelService LevelService => levelService;
        public PlayerControl PlayerControl => playerControl;
        public EndScreenUI EndScreenUI => endScreenUI;
        public Wallet Wallet => wallet;

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
            MainMenuState = new MainMenuState(this, eventService);
            RunningState = new RunningState(this, eventService);
            FightingState = new FightingState(this, eventService);
            EndState = new EndState(this, eventService);
        }

        private IEnumerator Start()
        {
            yield return AnimationManager.GetInstance().LoadAnimationAssetBundle($"{Application.streamingAssetsPath}/{AnimationTextureFileName}");
            currentState = MainMenuState;
            LevelService.StartLevelService();
            currentState.EnterState();
        }

        public void SwitchState(BaseGameState newGameState)
        {
            currentState = newGameState;
            currentState.EnterState();
        }
    }
}
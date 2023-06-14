using Extensions;
using Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class EndScreenUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup endScreen;
        [SerializeField] private CanvasGroup gameOverScreen;
        [SerializeField] private CanvasGroup victoryScreen;
        [SerializeField] private Button tryAgainButton;
        [SerializeField] private Button nextLevelButton;

        private EndScreenType currentEndScreen = EndScreenType.None;

        private EventService eventService;

        [Inject]
        private void Construct(EventService eventServiceReference)
        {
            eventService = eventServiceReference;
        }

        private void Start()
        {
            tryAgainButton.onClick.AddListener(TryAgainButtonPressed);
            nextLevelButton.onClick.AddListener(NextLevelButtonPress);
        }

        private void TryAgainButtonPressed()
        {
            eventService.InvokeTryAgainPressed();
        }

        private void NextLevelButtonPress()
        {
            eventService.InvokeNextLevelPressed();
        }

        public void ShowEndScreen(bool state)
        {
            endScreen.ShowCanvasGroup(state);
        }

        public void ShowGameOverScreen(bool state)
        {
            if (currentEndScreen == EndScreenType.Lose) return;
            gameOverScreen.ShowCanvasGroup(state);
            victoryScreen.ShowCanvasGroup(!state);
        }

        public void ShowVictoryScreen(bool state)
        {
            if (currentEndScreen == EndScreenType.Victory) return;
            victoryScreen.ShowCanvasGroup(state);
            gameOverScreen.ShowCanvasGroup(!state);
        }

        private void OnDestroy()
        {
            tryAgainButton.onClick.RemoveListener(TryAgainButtonPressed);
            nextLevelButton.onClick.RemoveListener(NextLevelButtonPress);
        }
    }
}
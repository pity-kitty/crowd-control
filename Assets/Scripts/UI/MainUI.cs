using System;
using Extensions;
using Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class MainUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup mainUiCanvasGroup;
        [SerializeField] private Button tapToStartButton;
        //TODO: Add user data elements

        private UserDataService userDataService;

        public event Action OnGameStarted;

        [Inject]
        private void Construct(UserDataService userDataServiceReference)
        {
            userDataService = userDataServiceReference;
        }
        
        private void Awake()
        {
            userDataService.LoadUserData();
        }

        private void Start()
        {
            tapToStartButton.onClick.AddListener(StartGame);
        }

        private void StartGame()
        {
            mainUiCanvasGroup.ShowCanvasGroup(false);
            OnGameStarted?.Invoke();
            //TODO: Change game state to running
        }

        private void OnDestroy()
        {
            tapToStartButton.onClick.RemoveListener(StartGame);
        }
    }
}
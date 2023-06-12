using Extensions;
using Services;
using TMPro;
using UnityEngine;
using Zenject;

namespace Player
{
    public class Wallet : MonoBehaviour
    {
        [SerializeField] private TMP_Text coinsValueText;
        [SerializeField] private CanvasGroup incomeCanvasGroup;
        [SerializeField] private TMP_Text incomeValueText;
        
        private int overallMoney;

        private int levelIncome = 0;
        
        private EventService eventService;
        private UserDataService userDataService;

        [Inject]
        private void Construct(EventService eventServiceReference, UserDataService userDataServiceReference)
        {
            eventService = eventServiceReference;
            userDataService = userDataServiceReference;
        }

        public int OverallMoney => overallMoney;
        public int LevelIncome => levelIncome;

        private void Start()
        {
            overallMoney = userDataService.CurrentUser.Money;
            coinsValueText.text = overallMoney.ToString();
            
            eventService.MoneyGained += AccumulateIncome;
        }

        private void AccumulateIncome(int income)
        {
            levelIncome += income;
            incomeValueText.text = $"+{levelIncome}";
        }

        public void ResetIncome()
        {
            overallMoney += levelIncome;
            levelIncome = 0;
            userDataService.CurrentUser.Money = overallMoney;
            userDataService.SaveUserData();
        }

        public void SetOverallMoney()
        {
            coinsValueText.text = overallMoney.ToString();
        }

        public void ShowIncome(bool state)
        {
            incomeValueText.text = $"+{levelIncome}";
            incomeCanvasGroup.ShowCanvasGroup(state);
        }

        private void OnDestroy()
        {
            eventService.MoneyGained -= AccumulateIncome;
        }
    }
}
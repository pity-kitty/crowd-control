using Models;
using UnityEngine;

namespace Services
{
    public class UserDataService
    {
        private const string MoneyKey = "Money";
        private const string StartCrowdCountKey = "StartCrowdCount";
        private const string LevelKey = "Level";

        private UserData currentUser;

        public UserData CurrentUser => currentUser;

        public void SaveUserData(UserData userData)
        {
            PlayerPrefs.SetInt(MoneyKey, userData.Money);
            PlayerPrefs.SetInt(StartCrowdCountKey, userData.StartCrowdCount);
            PlayerPrefs.SetInt(LevelKey, userData.Level);
            PlayerPrefs.Save();
        }

        public UserData LoadUserData()
        {
            var money = PlayerPrefs.GetInt(MoneyKey);
            var startCrowdCount = PlayerPrefs.GetInt(StartCrowdCountKey);
            if (startCrowdCount == 0) startCrowdCount = 1;
            var level = PlayerPrefs.GetInt(LevelKey);
            currentUser = new UserData(money, startCrowdCount, level);
            return currentUser;
        }
    }
}
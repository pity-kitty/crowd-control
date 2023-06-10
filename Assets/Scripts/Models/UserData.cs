namespace Models
{
    public class UserData
    {
        private int money;
        private int startCrowdCount;
        private int level;

        public int Money
        {
            get => money;
            set => money = value;
        }

        public int StartCrowdCount
        {
            get => startCrowdCount;
            set => startCrowdCount = value;
        }

        public int Level
        {
            get => level;
            set => level = value;
        }

        public UserData(int money, int startCrowdCount, int level)
        {
            this.money = money;
            this.startCrowdCount = startCrowdCount;
            this.level = level;
        }
    }
}
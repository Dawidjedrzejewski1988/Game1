namespace Game.Models
{
    public class DailyQuest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int RewardExperience { get; set; }
        public int RewardGold { get; set; }
        public DateTime ResetTime { get; set; }
    }
}

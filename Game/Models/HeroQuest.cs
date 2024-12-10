namespace Game.Models
{
    public class HeroQuest
    {
        public int HeroId { get; set; }
        public Hero Hero { get; set; }

        public int QuestId { get; set; }
        public Quest Quest { get; set; }

        public bool IsCompleted { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int RewardExperience { get; set; }
    }
}

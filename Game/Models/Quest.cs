namespace Game.Models
{
    public class Quest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Difficulty { get; set; }
        public int RewardExperience { get; set; }

        public bool IsCompleted { get; set; } = false;

        public ICollection<HeroQuest> HeroQuests { get; set; } = new List<HeroQuest>();
    }
}

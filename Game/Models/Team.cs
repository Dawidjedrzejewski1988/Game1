namespace Game.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Hero> Members { get; set; } = new List<Hero>();
        public ICollection<TeamQuest> TeamQuests { get; set; } = new List<TeamQuest>();
    }

    public class TeamQuest
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }

        public int QuestId { get; set; }
        public Quest Quest { get; set; }

        public bool IsCompleted { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}

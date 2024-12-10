namespace Game.Models
{
    public class Enemy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Health { get; set; }
        public int Strength { get; set; }
        public int Defense { get; set; } 
        public int RewardGold { get; set; } 
        public int RewardExperience { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
    }
}

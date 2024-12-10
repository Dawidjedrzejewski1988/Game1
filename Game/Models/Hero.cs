using System.ComponentModel.DataAnnotations;

namespace Game.Models
{
    public class Hero
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Imię postaci jest wymagane.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Musisz wybrać klasę postaci.")]
        public string Class { get; set; }

        public int Level { get; set; } = 1;
        public int Experience { get; set; } = 0;
        public int Strength { get; set; } = 10;
        public int Dexterity { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public int PointsToSpend { get; set; } = 0;
        public int Defense { get; set; } = 0;
        public int Health { get; set; } = 100;
        public int Money { get; set; } = 10;

        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<HeroQuest> HeroQuests { get; set; } = new List<HeroQuest>();
        public ICollection<Item> Items { get; set; } = new List<Item>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public int? LocationId { get; set; }
        public Location Location { get; set; }


    }
}

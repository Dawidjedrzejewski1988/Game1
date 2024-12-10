namespace Game.Models
{
    public class Guild
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Hero> Members { get; set; } = new List<Hero>();
    }
}

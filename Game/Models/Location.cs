namespace Game.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; } // Ścieżka do ikony dla lokacji
        public int X { get; set; } // Pozycja X na mapie
        public int Y { get; set; } // Pozycja Y na mapie
        public bool IsAccessible { get; set; } = true; // Czy lokacja jest dostępna

        public ICollection<Quest> Quests { get; set; } = new List<Quest>();
    }
}

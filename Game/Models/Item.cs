namespace Game.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int BonusStrength { get; set; }
        public int BonusDexterity { get; set; }
        public int BonusIntelligence { get; set; }
        public int BonusDefense { get; set; }
        public int BonusHealth { get; set; }
        public int Price { get; set; }
        public bool IsForSale { get; set; }
        public bool IsEquipped { get; set; }
        public int Quantity { get; set; }
        public int UpgradeLevel { get; set; } = 0;
    }
}

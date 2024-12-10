namespace Game.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int HeroId { get; set; }
        public Hero Hero { get; set; }
        public int? ItemId { get; set; }
        public Item Item { get; set; }
        public bool IsPurchase { get; set; } 
        public int Amount { get; set; } 
        public DateTime TransactionDate { get; set; }
    }
}

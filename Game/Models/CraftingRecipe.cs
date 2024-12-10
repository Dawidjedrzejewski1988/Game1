namespace Game.Models
{
    public class CraftingRecipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ResultItemId { get; set; }
        public Item ResultItem { get; set; }

        public ICollection<CraftingRecipeIngredient> Ingredients { get; set; } = new List<CraftingRecipeIngredient>();
    }

    public class CraftingRecipeIngredient
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public CraftingRecipe Recipe { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }

        public int Quantity { get; set; }
    }
}

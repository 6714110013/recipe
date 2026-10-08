using RecipeApp.Data;

namespace RecipeApp.Models
{
    public class RecipeSummaryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TimeToCook { get; set; }
        public string Category { get; set; } = string.Empty;
        public int NumberOfIngredients { get; set; }
        public int ReviewCount { get; set; }

        public static RecipeSummaryViewModel FromRecipe(Recipe recipe)
        {
            return new RecipeSummaryViewModel
            {
                Id = recipe.RecipeId,
                Name = recipe.Name,
                TimeToCook = $"{recipe.TimeToCook.Hours}hrs {recipe.TimeToCook.Minutes}mins",
            };
        }
    }
}
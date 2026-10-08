using RecipeApp.Data;

namespace RecipeApp.Models
{
    public class CreateRecipeCommand : EditRecipeBase
    {
        public IList<CreateIngredientCommand> Ingredients { get; set; } = new List<CreateIngredientCommand>();

        public Recipe ToRecipe()
        {
            return new Recipe
            {
                Name = Name,
                TimeToCook = new TimeSpan(TimeToCookHrs, TimeToCookMins, 0),
                Method = Method,
                Category = Category, // <--- แมปค่า Category ตรงนี้
                IsVegetarian = IsVegetarian,
                IsVegan = IsVegan,
                Ingredients = Ingredients?.Select(x => x.ToIngredient()).ToList() ?? new List<Ingredient>()
            };
        }
    }
}
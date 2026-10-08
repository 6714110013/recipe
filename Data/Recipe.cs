namespace RecipeApp.Data
{
    public class Recipe
    {
        public int RecipeId { get; set; }
        public required string Name { get; set; }
        public TimeSpan TimeToCook { get; set; }
        public bool IsDeleted { get; set; }
        public required string Method { get; set; }
        // เพิ่มบรรทัดนี้ใน Recipe.cs
        public ICollection<RecipeReview> Reviews { get; set; } = new List<RecipeReview>();
        
        // เพิ่มบรรทัดนี้ครับ (สำหรับเก็บหมวดหมู่อาหาร)
        public string Category { get; set; } = "ของคาว";

        public bool IsVegetarian { get; set; }
        public bool IsVegan { get; set; }
        public required ICollection<Ingredient> Ingredients { get; set; }
    }
}
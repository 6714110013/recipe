namespace RecipeApp.Models
{
    public class RecipeDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Category { get; set; }
        public string Method { get; set; }
        public IEnumerable<Item> Ingredients { get; set; }

        public double AverageRating { get; set; } // คะแนนเฉลี่ย (เช่น 4.5)
        public int ReviewCount { get; set; }      // จำนวนคนรีวิวทั้งหมด
        public IEnumerable<ReviewItemViewModel> Reviews { get; set; } = new List<ReviewItemViewModel>();

        public class Item
        {
            public string Name { get; set; }
            public string Quantity { get; set; }
        }
    }
}
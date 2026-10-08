using Microsoft.EntityFrameworkCore;
using RecipeApp.Data;
using RecipeApp.Models;

namespace RecipeApp
{
    public class RecipeService
    {
        readonly AppDbContext _context;
        readonly ILogger _logger;

        public RecipeService(AppDbContext context, ILoggerFactory factory)
        {
            _context = context;
            _logger = factory.CreateLogger<RecipeService>();
        }

        // อัปเดตรองรับการค้นหา (searchString) และ กรองหมวดหมู่ (category)
        // 1. เพิ่มพารามิเตอร์ sortBy = null ใน Signature
        public async Task<List<RecipeSummaryViewModel>> GetRecipes(string? searchString = null, string? category = null, string? sortBy = null)
        {
            var query = _context.Recipes.Where(r => !r.IsDeleted);

            // ค้นหาตามชื่อเมนู
            if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(r => r.Name.Contains(searchString));
        }

        // กรองตามหมวดหมู่อาหาร
        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(r => r.Category == category);
        }

        // 2. เพิ่มเงื่อนไขจัดเรียงเมนูยอดนิยม (คอมเมนต์มากที่สุด)
        if (sortBy == "popular")
        {
            query = query.OrderByDescending(r => r.Reviews.Count);
        }
        else
        {
            query = query.OrderByDescending(r => r.RecipeId);
        }

        return await query
            .Select(x => new RecipeSummaryViewModel
            {
                Id = x.RecipeId,
                Name = x.Name,
                Category = x.Category,
                TimeToCook = $"{x.TimeToCook.Hours}hrs {x.TimeToCook.Minutes}mins",
                ReviewCount = x.Reviews.Count // 3. ดึงจำนวนรีวิวไปใช้แสดงผล
            })
            .ToListAsync();
        }

        public async Task<RecipeDetailViewModel?> GetRecipeDetail(int id)
        {
            return await _context.Recipes
                .Where(x => x.RecipeId == id && !x.IsDeleted)
                .Select(x => new RecipeDetailViewModel
                {
                    Id = x.RecipeId,
                    Name = x.Name,
                    Category = x.Category,
                    Method = x.Method,
                    
                    // คำนวณคะแนนดาวเฉลี่ยและรายการรีวิว
                    AverageRating = x.Reviews.Any() ? Math.Round(x.Reviews.Average(r => r.Rating), 1) : 0,
                    ReviewCount = x.Reviews.Count,
                    Reviews = x.Reviews.OrderByDescending(r => r.CreatedAt).Select(r => new ReviewItemViewModel
                    {
                        UserName = r.UserId,
                        Rating = r.Rating,
                        Comment = r.Comment,
                        CreatedAt = r.CreatedAt
                    }),

                    Ingredients = x.Ingredients.Select(item => new RecipeDetailViewModel.Item
                    {
                        Name = item.Name,
                        Quantity = $"{item.Quantity} {item.Unit}"
                    })
                })
                .SingleOrDefaultAsync();
        }

        public async Task<UpdateRecipeCommand> GetRecipeForUpdate(int recipeId)
        {
            return await _context.Recipes
                .Where(x => x.RecipeId == recipeId)
                .Where(x => !x.IsDeleted)
                .Select(x => new UpdateRecipeCommand
                {
                    Id = x.RecipeId,
                    Name = x.Name,
                    Category = x.Category,
                    Method = x.Method,
                    TimeToCookHrs = x.TimeToCook.Hours,
                    TimeToCookMins = x.TimeToCook.Minutes,
                    IsVegan = x.IsVegan,
                    IsVegetarian = x.IsVegetarian,
                })
                .SingleOrDefaultAsync();
        }

        /// <summary>
        /// Create a new recipe
        /// </summary>
        public async Task<int> CreateRecipe(CreateRecipeCommand cmd)
        {
            var recipe = cmd.ToRecipe();
            _context.Add(recipe);
            await _context.SaveChangesAsync();
            return recipe.RecipeId;
        }

        /// <summary>
        /// Updates an existing recipe
        /// </summary>
        public async Task UpdateRecipe(UpdateRecipeCommand cmd)
        {
            var recipe = await _context.Recipes.FindAsync(cmd.Id);
            if (recipe == null) { throw new Exception("Unable to find the recipe"); }
            if (recipe.IsDeleted) { throw new Exception("Unable to update a deleted recipe"); }

            cmd.UpdateRecipe(recipe);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Marks an existing recipe as deleted
        /// </summary>
        public async Task DeleteRecipe(int recipeId)
        {
            var recipe = await _context.Recipes.FindAsync(recipeId);
            if (recipe is null) { throw new Exception("Unable to find recipe"); }

            recipe.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// เพิ่มรีวิวและคะแนนดาวสำหรับสูตรอาหาร
        /// </summary>
        public async Task AddReview(int recipeId, string userId, int rating, string? comment)
        {
            var review = new RecipeReview
            {
                RecipeId = recipeId,
                UserId = userId,
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.Now
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeApp.Models;

namespace RecipeApp.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly RecipeService _service;
        public IEnumerable<RecipeSummaryViewModel> Recipes { get; private set; } = new List<RecipeSummaryViewModel>();

        // เพิ่ม 2 ตัวแปรนี้เพื่อรับค่าจากฟอร์มค้นหาและตัวกรองใน Index.cshtml
        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SelectedCategory { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SortBy { get; set; }

        public async Task OnGetAsync()
        {
            Recipes = await _service.GetRecipes(SearchString, SelectedCategory, SortBy);
        }

        public IndexModel(RecipeService service)
        {
            _service = service;
        }
    }
}
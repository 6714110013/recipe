using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeApp.Models;

namespace RecipeApp.Pages.Recipes
{
    [Authorize]
    public class ViewModel : PageModel
    {
        private readonly RecipeService _service;

        public ViewModel(RecipeService service)
        {
            _service = service;
        }

        public RecipeDetailViewModel Recipe { get; set; } = default!;

        // เพิ่มตัวแปรสำหรับรับข้อมูลรีวิวจากฟอร์ม
        [BindProperty]
        public RecipeReviewViewModel NewReview { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var recipe = await _service.GetRecipeDetail(id);
            if (recipe is null)
            {
                return NotFound();
            }

            Recipe = recipe;
            NewReview.RecipeId = id;
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _service.DeleteRecipe(id);
            return RedirectToPage("/Index");
        }

        // เพิ่ม Handler สำหรับบันทึกรีวิว
        public async Task<IActionResult> OnPostReviewAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                var recipe = await _service.GetRecipeDetail(id);
                if (recipe is null) return NotFound();
                Recipe = recipe;
                return Page();
            }

            // ดึงชื่อผู้ใช้งานที่กำลังล็อกอินอยู่
            var userId = User.FindFirstValue(ClaimTypes.Name) ?? User.Identity?.Name ?? "Anonymous";

            // บันทึกลงฐานข้อมูลผ่าน Service
            await _service.AddReview(id, userId, NewReview.Rating, NewReview.Comment);

            // รีเฟรชหน้าเดิมเพื่อแสดงรีวิวใหม่
            return RedirectToPage(new { id });
        }
    }
}
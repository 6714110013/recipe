using System.ComponentModel.DataAnnotations;

namespace RecipeApp.Models
{
    public class RecipeReviewViewModel
    {
        public int RecipeId { get; set; }

        [Required(ErrorMessage = "กรุณาเลือกรอบคะแนนดาว")]
        [Range(1, 5, ErrorMessage = "คะแนนต้องอยู่ระหว่าง 1 - 5 ดาว")]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string? Comment { get; set; }
    }

    public class ReviewItemViewModel
    {
        public string UserName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
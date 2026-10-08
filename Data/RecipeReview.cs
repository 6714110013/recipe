using System.ComponentModel.DataAnnotations;

namespace RecipeApp.Data
{
    public class RecipeReview
    {
        public int RecipeReviewId { get; set; }

        public int RecipeId { get; set; }
        public Recipe? Recipe { get; set; }

        // เก็บ ID ของผู้รีวิว (จาก Identity / ApplicationUser)
        public string UserId { get; set; } = string.Empty;

        [Range(1, 5, ErrorMessage = "กรุณาให้คะแนนระหว่าง 1 ถึง 5 ดาว")]
        public int Rating { get; set; } // คะแนน 1-5 ดาว

        [MaxLength(1000, ErrorMessage = "ข้อความรีวิวต้องไม่เกิน 1,000 ตัวอักษร")]
        public string? Comment { get; set; } // ข้อความความคิดเห็น

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
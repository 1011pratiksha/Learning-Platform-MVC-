using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningPlatform.Models
{
    [Table("Mcq")]
    public class Mcq
    {
        // Primary key of MCQ table
        // Database column: mcqid
        [Key]
        [Column("mcqid")]
        public int Id { get; set; }


        // Stores the Material ID to which this question belongs
        // Database column: material_id
        [Column("material_id")]
        public int MaterialId { get; set; }


        // Stores the MCQ question
        // Database column: question
        [Required]
        [Column("question")]
        public string Question { get; set; }


        // Stores option 1
        // Database column: option1
        [Required]
        [Column("option1")]
        public string Option1 { get; set; }


        // Stores option 2
        // Database column: option2
        [Required]
        [Column("option2")]
        public string Option2 { get; set; }


        // Stores option 3
        // Database column: option3
        [Required]
        [Column("option3")]
        public string Option3 { get; set; }


        // Stores option 4
        // Database column: option4
        [Required]
        [Column("option4")]
        public string Option4 { get; set; }


        // Stores the correct answer
        // Database column: answer
        //
        // Example:
        // "Option 2"
        // or
        // "C#"
        [Required]
        [Column("answer")]
        public string Answer { get; set; }


        // 
        // NAVIGATION PROPERTY
        // 

        // Each MCQ belongs to one Material
        //
        // Mcq.MaterialId
        //        ↓
        // Material.Id (material_id)
        [ForeignKey("MaterialId")]
        public Material Material { get; set; }
    }
}
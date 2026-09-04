using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningPlatform.Models
{
    [Table("Material")]
    public class Material
    {
        // Primary key of Material table
        // Database column: material_id
        [Key]
        [Column("material_id")]
        public int Id { get; set; }


        // Stores the Master Course ID
        // Database column: mid
        [Column("mid")]
        public int MasterCourseId { get; set; }


        // Stores the Sub Course ID
        // Database column: sid
        [Column("sid")]
        public int SubCourseId { get; set; }


        // Stores the Topic ID
        // Database column: tid
        [Column("tid")]
        public int TopicId { get; set; }


        // Stores the uploaded assignment file path
        // Example:
        // /uploads/assignments/image.pdf
        [Column("assignment")]
        public string Assignment { get; set; }


        // 
        // NAVIGATION PROPERTIES
        // many to one 

        // Material belongs to one Master Course.
        [ForeignKey("MasterCourseId")]
        public MasterCourse MasterCourse { get; set; }


        // Material belongs to one Sub Course.
        [ForeignKey("SubCourseId")]
        public SubCourse SubCourse { get; set; }


        // Material belongs to one Topic.
        [ForeignKey("TopicId")]
        public Topic Topic { get; set; }


        // One Material can contain multiple MCQs
        //
        // Example:
        // Material 1
        //      Question 1
        //      Question 2
        //      Question 3
        public ICollection<Mcq> Mcqs { get; set; }
    }
}
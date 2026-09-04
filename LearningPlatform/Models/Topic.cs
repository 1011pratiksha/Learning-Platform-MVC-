using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningPlatform.Models
{
    [Table("Topic")]
    public class Topic
    {
        // Primary key of Topic table
        // Database column: tid
        [Key]
        [Column("tid")]
        public int Id { get; set; }


        // Stores the Master Course ID selected by Admin
        // Database column: mid
        [Column("mid")]
        public int MasterCourseId { get; set; }


        // Stores the Sub Course ID selected by Admin
        // Database column: sid
        [Column("sid")]
        public int SubCourseId { get; set; }


        // Stores the name of the topic
        // Database column: tname
        [Required]
        [Column("tname")]
        public string TopicName { get; set; }


        // Stores the video URL entered by Admin
        // Database column: videoUrl
        [Column("videoUrl")]
        public string VideoUrl { get; set; }


        // Stores the topic status
        // true  = Active
        // false = Inactive
        // Database column: tstatus
        [Column("tstatus")]
        public bool Status { get; set; }


        // Stores the uploaded thumbnail path
        // Example:
        // /uploads/topic-thumbnail.jpg
        [Column("tthumbnail")]
        public string ThumbnailPath { get; set; }


        // 
        // NAVIGATION PROPERTIES
        // 

        // Topic belongs to one Master Course.
        [ForeignKey("MasterCourseId")]
        public MasterCourse MasterCourse { get; set; }


        // Topic belongs to one Sub Course.
        [ForeignKey("SubCourseId")]
        public SubCourse SubCourse { get; set; }


        // One Topic can contain many Materials.
        //
        // Topic
        //    Material 1
        //    Material 2
        //    Material 3
        //    Material 4
        //
        // MCQs are NOT directly connected to Topic anymore
        // They belong to Material
        public ICollection<Material> Materials { get; set; }
    }
} 
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearning.Models
{
    [Table("Master_course")]
    public class MasterCourse
    {
        [Key]
        [Column("mid")]
        public int Mid { get; set; }

        [Column("mname")]
        public string Mname { get; set; } 

        [Column("mstatus")]
        public string Mstatus { get; set; } 

        [Column("mthumbnail")]
        public string Mthumbnail { get; set; } 

        [Column("createdAt")]
        public DateTime CreatedAt { get; set; }

        [Column("createdBy")]
        public string CreatedBy { get; set; } 
    }
}
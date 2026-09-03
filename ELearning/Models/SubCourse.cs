using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearning.Models
{
    [Table("sub_course")]
    public class SubCourse
    {
        [Key]
        [Column("sid")]
        public int Sid { get; set; }

        [Column("mid")]
        public int Mid { get; set; }

        [Column("sname")]
        public string Sname { get; set; } 

        [Column("sstatus")]
        public string Sstatus { get; set; } 

        [Column("samount")]
        public decimal Samount { get; set; }

        [Column("createdAt")]
        public DateTime CreatedAt { get; set; }

        [Column("createdBy")]
        public string CreatedBy { get; set; } 
    }
}
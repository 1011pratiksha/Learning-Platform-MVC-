using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_platform.Models
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
        public string SName { get; set; } = string.Empty;

        [Column("sstatus")]
        public string SStatus { get; set; } = string.Empty;

        [Column("samount")]
        public decimal SAmount { get; set; }

        [Column("createdAt")]
        public DateTime CreatedAt { get; set; }

        [Column("createdBy")]
        public string CreatedBy { get; set; }
        [ForeignKey("Mid")]
        public MasterCourse MasterCourse { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearning.Models
{
    [Table("Topic")]
    public class Topic
    {
        [Key]
        [Column("tid")]
        public int Tid { get; set; }

        [Column("mid")]
        public int Mid { get; set; }

        [Column("sid")]
        public int Sid { get; set; }

        [Column("tname")]
        public string Tname { get; set; } 

        [Column("videoUrl")]
        public string VideoUrl { get; set; } 

        [Column("tstatus")]
        public string Tstatus { get; set; } 

        [Column("tthumbnail")]
        public string Tthumbnail { get; set; } 
    }
}
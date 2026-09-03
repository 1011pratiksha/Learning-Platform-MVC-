using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearning.Models
{
    [Table("AdminSubscription")]
    public class AdminSubscription
    {
        [Key]
        [Column("sub_id")]
        public int SubId { get; set; }

        [Column("sub_type")]
        public string SubType { get; set; }

        [Column("mid")]
        public int Mid { get; set; }

        [Column("sid")]
        public int Sid { get; set; }

        [Column("sub_amount")]
        public decimal SubAmount { get; set; }

        [Column("subStatus")]
        public string SubStatus { get; set; } 

        [Column("subThumbnail")]
        public string SubThumbnail { get; set; } 
    }
}
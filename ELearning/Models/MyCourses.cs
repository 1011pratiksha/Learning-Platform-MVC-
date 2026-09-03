using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearning.Models
{
    [Table("my_courses")]
    public class MyCourses
    {
        [Column("sid")]
        public int Sid { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }
    }
}
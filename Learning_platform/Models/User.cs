using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_platform.Models
{
    [Table("user")]
    public class User
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("user_name")]
        public string UserName { get; set; } = string.Empty;

        [Column("user_email")]
        public string UserEmail { get; set; } = string.Empty;

        [Column("user_password")]
        public string UserPassword { get; set; } = string.Empty;
   
}

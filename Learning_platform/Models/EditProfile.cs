using System.ComponentModel.DataAnnotations;

namespace Learning_platform.Models
{
    public class EditProfile
    {
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string UserEmail { get; set; } = string.Empty;
    }
}

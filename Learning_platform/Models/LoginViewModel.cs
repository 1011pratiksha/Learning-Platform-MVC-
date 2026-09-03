using System.ComponentModel.DataAnnotations;

namespace Learning_platform.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}

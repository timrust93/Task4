using System.ComponentModel.DataAnnotations;

namespace Task4.Models
{
    public class LoginPoco
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
    }
}

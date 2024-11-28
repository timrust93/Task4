using System.ComponentModel.DataAnnotations;

namespace Task4.Models
{
    public class UserRegisterModel
    {
        [Required]
        public string Login { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;

    }
}

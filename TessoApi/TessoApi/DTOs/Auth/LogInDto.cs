using System.ComponentModel.DataAnnotations;

namespace TessoApi.DTOs.Auth
{
    public class LogInDto
    {
        [Required(ErrorMessage = "User ID / Email is required.")]
        public required string UserId { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public required string Password { get; set; }
        public bool RememberMe { get; set; } = false;
    }
}

using System.ComponentModel.DataAnnotations;

namespace TessoApi.DTOs.Auth
{
    public class LogInDto
    {
        [Required]
        public required string UserId { get; set; }

        [Required]
        public required string Password { get; set; }
        public bool RememberMe { get; set; } = false;
    }
}

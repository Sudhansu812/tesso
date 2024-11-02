using System.ComponentModel.DataAnnotations;

namespace TessoApi.DTOs.Auth
{
    public class RegisterDto
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }
}

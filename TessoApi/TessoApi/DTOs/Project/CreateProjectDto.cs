using System.ComponentModel.DataAnnotations;

namespace TessoApi.DTOs.Project
{
    public class CreateProjectDto
    {
        [Required(ErrorMessage = "The project name is required."), MaxLength(64)]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}

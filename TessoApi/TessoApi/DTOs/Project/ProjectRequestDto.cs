using System.ComponentModel.DataAnnotations;

namespace TessoApi.DTOs.Project
{
    public class ProjectRequestDto
    {
        [Required(ErrorMessage = "The project ID is required.")]
        public Guid ProjectId { get; set; }
    }
}

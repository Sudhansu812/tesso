using System.ComponentModel.DataAnnotations.Schema;

namespace TessoApi.DTOs.Project
{
    public class UpdateProjectDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid OwnerId { get; set; }
    }
}

using TessoApi.DTOs.Project;

namespace TessoApi.DTOs.Collection
{
    public class ReadCollectionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ReadProjectDto>? Projects { get; set; }
    }
}

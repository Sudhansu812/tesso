namespace TessoApi.DTOs.Project
{
    public class ProjectOwnerReadDto
    {
        public Guid ProjectId { get; set; }
        public List<Guid> OwnerIds { get; set; }
    }
}

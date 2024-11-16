using TessoApi.DTOs.Project;
using TessoApi.Models.Http;

namespace TessoApi.Services.Interfaces
{
    public interface IProjectService
    {
        public Task<CustomHttpResponse<ReadProjectDto>> Create(CreateProjectDto project);
        public Task<CustomHttpResponse<ReadProjectDto>> GetProject(Guid projectId);
        public Task<CustomHttpResponse<List<ReadProjectDto>>> GetUserProjects(Guid userId);
    }
}

using Microsoft.AspNetCore.JsonPatch;
using TessoApi.DTOs.Project;
using TessoApi.Models.Http;

namespace TessoApi.Services.Interfaces
{
    public interface IProjectService
    {
        public Task<CustomHttpResponse<ReadProjectDto>> Create(CreateProjectDto project);
        public Task<CustomHttpResponse<ReadProjectDto>> GetProject(Guid projectId);
        public Task<CustomHttpResponse<List<ReadProjectDto>>> GetUserProjects(Guid userId);
        public Task<CustomHttpResponse<ReadProjectDto>> UpdateProject(Guid projectId, UpdateProjectDto projectDto);
        public Task<CustomHttpResponse<ReadProjectDto>> PatchProject(Guid projectId, JsonPatchDocument<UpdateProjectDto> projectPatch);
        public Task<CustomHttpResponse<bool>> DeleteProject(Guid projectId);
        public Task<CustomHttpResponse<ProjectOwnerReadDto>> AssignProjectOwner(ProjectOwnerCreateDto projectOwner);
    }
}

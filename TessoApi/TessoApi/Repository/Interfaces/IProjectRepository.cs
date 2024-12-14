using TessoApi.Models;
using TessoApi.Models.Http;

namespace TessoApi.Repository.Interfaces
{
    public interface IProjectRepository
    {
        public Task<Project> Create(Project project);
        public Task<Project> GetProject(Guid projectId);
        public Task<bool> UserProjectExist(Guid userId, string projectName);
        public Task<List<Project>> GetUserProjects(Guid userId);
        public Task<bool> SaveChangesAsync();
        public Task<bool> DeleteProject(Project project);
    }
}

using Microsoft.EntityFrameworkCore;
using TessoApi.Exceptions;
using TessoApi.Models;
using TessoApi.Repository.DB;
using TessoApi.Repository.Interfaces;

namespace TessoApi.Repository
{
    public class ProjectRepository(AppDbContext context) : IProjectRepository
    {
        private readonly AppDbContext _db = context ?? throw new DbContextException("Database context not found.");

        public async Task<Project> Create(Project project)
        {
            await _db.Projects.AddAsync(project);
            await SaveChangesAsync();
            return project;
        }

        public async Task<Project> GetProject(Guid projectId)
        {
            Project? project = await _db.Projects.FirstOrDefaultAsync(p => p.Id.Equals(projectId));
            if (project == null)
            {
                throw new KeyNotFoundException();
            }
            return project;
        }

        public async Task<bool> UserProjectExist(Guid userId, string projectName)
        {
            return await _db.Projects.AnyAsync(p => (p.CreatorId.Equals(userId) || p.OwnerId.Equals(userId)) && p.Name.Equals(projectName));
        }

        public async Task<List<Project>> GetUserProjects(Guid userId)
        {
            return await _db.Projects.Where(p => p.CreatorId.Equals(userId) || p.OwnerId.Equals(userId)).ToListAsync();
        }

        public async Task<bool> SaveChangesAsync() => await _db.SaveChangesAsync() > 0;
    }
}

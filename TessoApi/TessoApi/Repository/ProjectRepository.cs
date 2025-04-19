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
            return await _db.Projects.AnyAsync(p => (p.CreatorId.Equals(userId)) && p.Name.Equals(projectName));
        }

        public async Task<List<Project>> GetUserProjects(Guid userId)
        {
            IQueryable<Project> projects = _db.Projects.Where(p => p.CreatorId.Equals(userId));
            projects.Union(
                _db.ProjectOwners
                .Where(po => po.UserId.Equals(userId))
                .Include(proj => proj.Project)
                .AsSplitQuery()
                .Select(po => po.Project)
                );

            return await projects.ToListAsync();
        }

        public async Task<List<Guid>> GetProjectOwnerIds(Guid projectId)
        {
            return await _db.ProjectOwners.AsNoTracking().Where(po => po.ProjectId.Equals(projectId)).Select(po => po.UserId).ToListAsync();
        }

        public async Task<bool> DeleteProject(Project project)
        {
            if (project == null)
            {
                throw new KeyNotFoundException();
            }
            _db.Projects.Remove(project);
            return await SaveChangesAsync();
        }

        public async Task<ProjectOwner> AssignProjectOwner(ProjectOwner projectOwner)
        {
            await _db.ProjectOwners.AddAsync(projectOwner);
            await SaveChangesAsync();
            return projectOwner;
        }

        public async Task<bool> ProjectExist(Guid projectId)
        {
            return (await _db.Projects.FindAsync(projectId)) is not null;
        }

        public async Task<bool> SaveChangesAsync() => await _db.SaveChangesAsync() > 0;
    }
}

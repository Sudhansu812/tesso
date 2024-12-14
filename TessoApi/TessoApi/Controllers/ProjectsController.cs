using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TessoApi.DTOs.Project;
using TessoApi.Services.Interfaces;
using TessoApi.Models.Http;
using Azure;
using Microsoft.AspNetCore.JsonPatch;

namespace TessoApi.Controllers
{
    [ApiController, Route("api/[controller]"), Authorize]
    public class ProjectsController(IProjectService projectService) : ControllerBase
    {
        private readonly IProjectService _projectService = projectService;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProjectDto project)
        {
            CustomHttpResponse<ReadProjectDto> response = await _projectService.Create(project);
            return StatusCode((int)response.StatusCode, response.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetProject([FromQuery] Guid projectId)
        {
            CustomHttpResponse<ReadProjectDto> response = await _projectService.GetProject(projectId);
            return StatusCode((int)response.StatusCode, response.Data);
        }

        [HttpGet("user-projects")]
        public async Task<IActionResult> GetUserProjects([FromQuery] Guid userId)
        {
            CustomHttpResponse<List<ReadProjectDto>> response = await _projectService.GetUserProjects(userId);
            return StatusCode((int)response.StatusCode, response.Data);
        }

        [HttpPut("{projectId}")]
        public async Task<IActionResult> UpdateUserProject(Guid projectId, [FromBody] UpdateProjectDto project)
        {
            CustomHttpResponse<ReadProjectDto> response = await _projectService.UpdateProject(projectId, project);
            return StatusCode((int)response.StatusCode, response.Data);
        }

        [HttpPatch("{projectId}")]
        public async Task<IActionResult> PatchUserProject(Guid projectId, [FromBody] JsonPatchDocument<UpdateProjectDto> project)
        {
            CustomHttpResponse<ReadProjectDto> response = await _projectService.PatchProject(projectId, project);
            return StatusCode((int)response.StatusCode, response.Data);
        }

        [HttpDelete("{projectId}")]
        public async Task<IActionResult> DeleteUserProject(Guid projectId)
        {
            CustomHttpResponse<bool> response = await _projectService.DeleteProject(projectId);
            return StatusCode((int)response.StatusCode, response.Data);
        }
    }
}

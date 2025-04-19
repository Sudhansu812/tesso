using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TessoApi.DTOs.Collection;
using TessoApi.DTOs.Project;
using TessoApi.Models.Http;
using TessoApi.Services.Interfaces;

namespace TessoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectionsController(ICollectionService collectionService) : ControllerBase
    {
        private readonly ICollectionService _collectionService = collectionService;

        [HttpPost]
        public async Task<IActionResult> CreateCollection([FromBody] CreateCollectionDto collection)
        {
            CustomHttpResponse<ReadCollectionDto> response = await _collectionService.CreateCollection(collection);
            return StatusCode((int)response.StatusCode, response.Data);
        }

        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetCollectionsByUser(Guid userId)
        {
            CustomHttpResponse<List<ReadCollectionDto>?> response = await _collectionService.GetCollectionsByUser(userId);
            return StatusCode((int)response.StatusCode, response.Data);
        }

        [HttpGet("projects/{collectionId:guid}")]
        public async Task<IActionResult> GetCollectionProjects(Guid collectionId)
        {
            CustomHttpResponse<List<CollectionProjectsDto>?> response = await _collectionService.GetCollectionProjects(collectionId);
            return StatusCode((int)response.StatusCode, response.Data);
        }

        [HttpPost("link-project")]
        public async Task<IActionResult> LinkProjectToCollection([FromBody] ProjectToCollectionDto project)
        {
            CustomHttpResponse<List<CollectionProjectsDto>?> response = await _collectionService.LinkProjectToCollection(project.CollectionId, project.ProjectId);
            return StatusCode((int)response.StatusCode, response.Data);
        }

        [HttpPost("unlink-project")]
        public async Task<IActionResult> UnlinkProjectFromCollection([FromBody] ProjectToCollectionDto project)
        {
            CustomHttpResponse<bool> response = await _collectionService.UnlinkProjectFromCollection(project.CollectionId, project.ProjectId);
            return StatusCode((int)response.StatusCode, response.Data);
        }
    }
}

using TessoApi.DTOs.Collection;
using TessoApi.Models;
using TessoApi.Models.Http;

namespace TessoApi.Services.Interfaces
{
    public interface ICollectionService
    {
        public Task<CustomHttpResponse<ReadCollectionDto>> CreateCollection(CreateCollectionDto collection);
        public Task<CustomHttpResponse<ReadCollectionDto>?> GetCollection(Guid collectionId);
        public Task<CustomHttpResponse<List<ReadCollectionDto>?>> GetCollectionsByUser(Guid userId);
        public Task<CustomHttpResponse<ReadCollectionDto>> UpdateCollection(UpdateCollectionDto collection);
        public Task<CustomHttpResponse<bool>> DeleteCollection(Guid collectionId);
        public Task<CustomHttpResponse<bool>> UnlinkProjectFromCollection(Guid collectionId, Guid projectId);
        public Task<CustomHttpResponse<List<CollectionProjectsDto>?>> LinkProjectToCollection(Guid collectionId, Guid projectId);
        public Task<CustomHttpResponse<List<CollectionProjectsDto>?>> GetCollectionProjects(Guid collectionId);
    }
}

using TessoApi.Models;

namespace TessoApi.Repository.Interfaces
{
    public interface ICollectionRepository
    {
        public Task<Collection> CreateCollection(Collection collection);
        public Task<bool> LinkUserToCollection(Collection collection, Guid userID);
        public Task<Collection?> GetCollection(Guid collectionId);
        public Task<List<Collection>> GetCollectionsByUser(Guid userId);
        public Task<Collection> UpdateCollection(Collection collection);
        public Task<bool> DeleteCollection(Guid collectionId);
        public Task<bool> UnlinkProjectFromCollection(Guid collectionId, Guid projectID);
        public Task<CollectionItem> LinkProjectToCollection(Guid collectionId, Guid projectId);
        public Task<bool> CollectionExist(Guid collectionId);
        public Task<List<Project>> GetCollectionProjects(Guid collectionId);
    }
}

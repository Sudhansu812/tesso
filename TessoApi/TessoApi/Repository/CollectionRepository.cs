using Microsoft.EntityFrameworkCore;
using TessoApi.Models;
using TessoApi.Repository.DB;
using TessoApi.Repository.Interfaces;

namespace TessoApi.Repository
{
    public class CollectionRepository(AppDbContext context) : ICollectionRepository
    {
        private readonly AppDbContext _db = context;

        public async Task<Collection> CreateCollection(Collection collection)
        {
            await _db.Collections.AddAsync(collection);
            await _db.SaveChangesAsync();
            return collection;
        }

        public async Task<bool> LinkUserToCollection(Collection collection, Guid userID)
        {
            CollectionUser collectionUser = new CollectionUser
            {
                CollectionId = collection.Id,
                HasDeleteAccess = true,
                IsOwner = true,
                IsReadOnly = false,
                UserId = userID
            };
            await _db.CollectionUsers.AddAsync(collectionUser);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<List<Project>> GetCollectionProjects(Guid collectionId)
        {
            IQueryable<Project> projects = _db.Collections.Where(x => x.Id.Equals(collectionId)).Include(x => x.Projects).SelectMany(x => x.Projects);
            return await projects.ToListAsync();
        }

        public async Task<Collection?> GetCollection(Guid collectionId)
        {
            return await _db.Collections.FirstOrDefaultAsync(c => c.Id.Equals(collectionId));
        }

        public async Task<List<Collection>> GetCollectionsByUser(Guid userId)
        {
            IQueryable<Collection> collectionIds = _db.CollectionUsers.Where(c => c.UserId.Equals(userId))
                                                                           .Include(c => c.Collection)
                                                                           .Select(c => c.Collection);
            return await collectionIds.ToListAsync();
        }

        public async Task<Collection> UpdateCollection(Collection collection)
        {
            _db.Collections.Update(collection);
            await _db.SaveChangesAsync();
            return collection;
        }

        public async Task<bool> DeleteCollection(Guid collectionId)
        {
            Collection? collection = await _db.Collections.FirstOrDefaultAsync(c => c.Id.Equals(collectionId));
            if (collection == null)
            {
                return false;
            }
            _db.Collections.Remove(collection);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnlinkProjectFromCollection(Guid collectionId, Guid projectID)
        {
            CollectionItem? collectionItem = await _db.CollectionItems.FirstOrDefaultAsync(ci => ci.CollectionId.Equals(collectionId) && ci.ProjectId.Equals(projectID));
            if (collectionItem is null)
            {
                return true;
            }
            _db.CollectionItems.Remove(collectionItem);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<CollectionItem> LinkProjectToCollection(Guid collectionId, Guid projectId)
        {
            CollectionItem collectionItem = new CollectionItem
            {
                CollectionId = collectionId,
                ProjectId = projectId
            };
            await _db.CollectionItems.AddAsync(collectionItem);
            return collectionItem;
        }

        public async Task<bool> CollectionExist(Guid collectionId)
        {
            return (await _db.Collections.FindAsync(collectionId)) is not null;
        }
    }
}

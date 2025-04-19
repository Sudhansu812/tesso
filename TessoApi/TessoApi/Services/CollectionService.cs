using Microsoft.IdentityModel.Tokens;
using System.Net;
using TessoApi.DTOs.Collection;
using TessoApi.DTOs.Project;
using TessoApi.Exceptions;
using TessoApi.Models;
using TessoApi.Models.Http;
using TessoApi.Repository.Interfaces;
using TessoApi.Services.Interfaces;

namespace TessoApi.Services
{
    public class CollectionService(ICollectionRepository collectionRepository, IProjectRepository projectRepository, IUserServices userServices) : ICollectionService
    {
        private readonly ICollectionRepository _collectionRepository = collectionRepository;
        private readonly IProjectRepository _projectRepository = projectRepository;
        private readonly IUserServices _userServices = userServices;

        public async Task<CustomHttpResponse<ReadCollectionDto>> CreateCollection(CreateCollectionDto collection)
        {
            try
            {
                Collection col = new Collection
                {
                    Name = collection.Name,
                    Description = collection.Description
                };

                await _collectionRepository.CreateCollection(col);
                await _collectionRepository.LinkUserToCollection(col, await _userServices.GetUserId());

                return new CustomHttpResponse<ReadCollectionDto>
                {
                    StatusCode = HttpStatusCode.Created,
                    Data = new ReadCollectionDto
                    {
                        Id = col.Id,
                        Name = col.Name,
                        Description = col.Description
                    },
                    Error = null
                };
            }
            catch (Exception e)
            {
                return new CustomHttpResponse<ReadCollectionDto>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null,
                    Error = e.Message
                };
            }
        }

        public async Task<CustomHttpResponse<ReadCollectionDto>?> GetCollection(Guid collectionId)
        {
            try
            {
                Collection? collection = await _collectionRepository.GetCollection(collectionId);
                if (collection is null)
                {
                    throw new CollectionNotFoundException("The specified collection was not found.");
                }
                return new CustomHttpResponse<ReadCollectionDto>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = new ReadCollectionDto
                    {
                        Id = collection.Id,
                        Name = collection.Name,
                        Description = collection.Description
                    },
                    Error = null
                };
            }
            catch (CollectionNotFoundException cnfe)
            {
                return new CustomHttpResponse<ReadCollectionDto>
                {
                    Data = null,
                    StatusCode = HttpStatusCode.NotFound,
                    Error = cnfe.Message
                };
            }
            catch (Exception e)
            {
                return new CustomHttpResponse<ReadCollectionDto>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null,
                    Error = e.Message
                };
            }
        }

        public async Task<CustomHttpResponse<List<ReadCollectionDto>?>> GetCollectionsByUser(Guid userId)
        {
            try
            {
                List<ReadCollectionDto>? collections = (await _collectionRepository.GetCollectionsByUser(userId)).Select(c => new ReadCollectionDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Projects = null
                }).ToList();

                if (collections is null || collections.Count <= 0)
                {
                    throw new CollectionNotFoundException("The specified collection was not found.");
                }
                return new CustomHttpResponse<List<ReadCollectionDto>?>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = collections,
                    Error = null
                };
            }
            catch (ArgumentNullException ane)
            {
                return new CustomHttpResponse<List<ReadCollectionDto>?>
                {
                    Data = null,
                    StatusCode = HttpStatusCode.NotFound,
                    Error = ane.Message
                };
            }
            catch (CollectionNotFoundException cnfe)
            {
                return new CustomHttpResponse<List<ReadCollectionDto>?>
                {
                    Data = null,
                    StatusCode = HttpStatusCode.NotFound,
                    Error = cnfe.Message
                };
            }
            catch (Exception e)
            {
                return new CustomHttpResponse<List<ReadCollectionDto>?>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null,
                    Error = e.Message
                };
            }
        }

        public async Task<CustomHttpResponse<ReadCollectionDto>> UpdateCollection(UpdateCollectionDto collection)
        {
            try
            {
                if (collection is null)
                {
                    throw new InvalidDataException("Collection details are empty.");
                }
                if (collection.Name.IsNullOrEmpty())
                {
                    throw new InvalidDataException("Collection name cannot be empty.");
                }

                Collection collectionEntity = new Collection
                {
                    Id = collection.Id,
                    Name = collection.Name,
                    Description = collection.Description
                };

                collectionEntity = await _collectionRepository.UpdateCollection(collectionEntity);
                return new CustomHttpResponse<ReadCollectionDto>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = new ReadCollectionDto
                    {
                        Id = collectionEntity.Id,
                        Name = collectionEntity.Name,
                        Description = collectionEntity.Description
                    },
                    Error = null
                };
            }
            catch (InvalidDataException ioe)
            {
                return new CustomHttpResponse<ReadCollectionDto>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = null,
                    Error = ioe.Message
                };
            }
            catch (Exception e)
            {
                return new CustomHttpResponse<ReadCollectionDto>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null,
                    Error = e.Message
                };
            }
        }

        public async Task<CustomHttpResponse<bool>> DeleteCollection(Guid collectionId)
        {
            try
            {
                return new CustomHttpResponse<bool>
                {
                    StatusCode = HttpStatusCode.NoContent,
                    Data = await _collectionRepository.DeleteCollection(collectionId),
                    Error = null
                };
            }
            catch (Exception e)
            {
                return new CustomHttpResponse<bool>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = false,
                    Error = e.Message
                };
            }
        }

        public async Task<CustomHttpResponse<bool>> UnlinkProjectFromCollection(Guid collectionId, Guid projectId)
        {
            try
            {
                if (!await CollectionProjectExist(collectionId, projectId))
                {
                    throw new CollectionNotFoundException("The collection or project was not found.");
                }
                return new CustomHttpResponse<bool>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = await _collectionRepository.UnlinkProjectFromCollection(collectionId, projectId),
                    Error = null
                };
            }
            catch (CollectionNotFoundException cnfe)
            {
                return new CustomHttpResponse<bool>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Data = false,
                    Error = cnfe.Message
                };
            }
            catch (Exception e)
            {
                return new CustomHttpResponse<bool>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = false,
                    Error = e.Message
                };
            }
        }

        public async Task<CustomHttpResponse<List<CollectionProjectsDto>?>> LinkProjectToCollection(Guid collectionId, Guid projectId)
        {
            try
            {
                if (!await CollectionProjectExist(collectionId, projectId))
                {
                    throw new CollectionNotFoundException("The collection or project was not found.");
                }
                await _collectionRepository.LinkProjectToCollection(collectionId, projectId);
                List<Project> projects = await _collectionRepository.GetCollectionProjects(collectionId);

                return new CustomHttpResponse<List<CollectionProjectsDto>?>
                {
                    StatusCode = HttpStatusCode.Created,
                    Data = projects.Select(c => new CollectionProjectsDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description
                    }).ToList(),
                    Error = null
                };
            }
            catch (CollectionNotFoundException cnfe)
            {
                return new CustomHttpResponse<List<CollectionProjectsDto>?>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Data = null,
                    Error = cnfe.Message
                };
            }
            catch (Exception e)
            {
                return new CustomHttpResponse<List<CollectionProjectsDto>?>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null,
                    Error = e.Message
                };
            }
        }

        public async Task<CustomHttpResponse<List<CollectionProjectsDto>?>> GetCollectionProjects(Guid collectionId)
        {
            try
            {
                if (!await _collectionRepository.CollectionExist(collectionId))
                {
                    throw new CollectionNotFoundException("The collection was not found.");
                }

                List<Project> projects = await _collectionRepository.GetCollectionProjects(collectionId);
                if (projects is null || projects.Count == 0)
                {
                    return new CustomHttpResponse<List<CollectionProjectsDto>?>
                    {
                        StatusCode = HttpStatusCode.OK,
                        Data = new (),
                        Error = null
                    };
                }

                return new CustomHttpResponse<List<CollectionProjectsDto>?>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = projects.Select(c => new CollectionProjectsDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description
                    }).ToList(),
                    Error = null
                };
            }
            catch (CollectionNotFoundException cnfe)
            {
                return new CustomHttpResponse<List<CollectionProjectsDto>?>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Data = null,
                    Error = cnfe.Message
                };
            }
            catch (Exception e)
            {
                return new CustomHttpResponse<List<CollectionProjectsDto>?>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null,
                    Error = e.Message
                };
            }
        }

        public async Task<bool> CollectionProjectExist(Guid collectionId, Guid projectId)
        {
            return (await _collectionRepository.CollectionExist(collectionId) && await _projectRepository.ProjectExist(projectId));
        }
    }
}

﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using TessoApi.DTOs.Auth;
using TessoApi.DTOs.Project;
using TessoApi.Exceptions;
using TessoApi.Helpers.Interfaces;
using TessoApi.Models;
using TessoApi.Models.Helper;
using TessoApi.Models.Http;
using TessoApi.Repository.Interfaces;
using TessoApi.Services.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TessoApi.Services
{
    public class ProjectService(IProjectRepository projectRepository, IObjectValidationHelper objectValidationHelper, IUserServices userServices) : IProjectService
    {
        private readonly IProjectRepository _projectRepository = projectRepository;
        private readonly IObjectValidationHelper _objectValidationHelper = objectValidationHelper;
        private readonly IUserServices _userServices = userServices;

        public async Task<CustomHttpResponse<ReadProjectDto>> Create(CreateProjectDto projectInput)
        {
            try
            {
                ObjectValidationResult validationResult = _objectValidationHelper.ValidateObject(projectInput);
                if (!validationResult.IsValid)
                {
                    throw new ArgumentNullException(validationResult.Error);
                }
                Guid userId = await _userServices.GetUserId();
                Project project = new Project
                {
                    Name = projectInput.Name,
                    Description = projectInput.Description,
                    CreatorId = userId,
                    OwnerId = userId
                };

                project = await _projectRepository.Create(project);

                string creatorFullName = await _userServices.GetUserFullName(userId);
                return new CustomHttpResponse<ReadProjectDto>
                {
                    StatusCode = HttpStatusCode.Created,
                    Data = new ReadProjectDto
                    {
                        Name = projectInput.Name,
                        Description = projectInput.Description,
                        Creator = creatorFullName,
                        Owner = creatorFullName
                    },
                    Error = null
                };
            }
            catch (ProjectAlreadyExistException pae)
            {
                return new CustomHttpResponse<ReadProjectDto>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = null,
                    Error = pae.Message
                };
            }
            catch (UserNotFoundException unf)
            {
                return new CustomHttpResponse<ReadProjectDto>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null,
                    Error = unf.Message
                };
            }
            catch (JwtTokenExtractionException jte)
            {
                return new CustomHttpResponse<ReadProjectDto>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null,
                    Error = jte.Message
                };
            }
            catch (ArgumentNullException ane)
            {
                return new CustomHttpResponse<ReadProjectDto>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = null,
                    Error = ane.Message
                };
            }
            catch (DbUpdateException due)
            {
                return new CustomHttpResponse<ReadProjectDto>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null,
                    Error = due.Message
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse<ReadProjectDto>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null,
                    Error = ex.Message
                };
            }
        }

        public async Task<CustomHttpResponse<ReadProjectDto>> GetProject(Guid projectId)
        {
            try
            {
                Project project = await _projectRepository.GetProject(projectId);

                string creatorFullName = await _userServices.GetUserFullName(project.CreatorId);
                string ownerFullName = await _userServices.GetUserFullName(project.OwnerId);

                return new CustomHttpResponse<ReadProjectDto>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = new ReadProjectDto
                    {
                        Name = project.Name,
                        Description = project.Description,
                        Creator = creatorFullName,
                        Owner = ownerFullName
                    },
                    Error = null
                };
            }
            catch (KeyNotFoundException knf)
            {
                return new CustomHttpResponse<ReadProjectDto>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Data = null,
                    Error = knf.Message
                };
            }
            catch (DbContextException ane)
            {
                return new CustomHttpResponse<ReadProjectDto>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = null,
                    Error = ane.Message
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse<ReadProjectDto>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null,
                    Error = ex.Message
                };
            }
        }

        public async Task<CustomHttpResponse<List<ReadProjectDto>>> GetUserProjects(Guid userId)
        {
            try
            {
                List<Project> projects = await _projectRepository.GetUserProjects(userId);
                List<ReadProjectDto> projectDtos = new List<ReadProjectDto>();

                foreach (var project in projects)
                {
                    string creatorFullName = await _userServices.GetUserFullName(project.CreatorId);
                    string ownerFullName = await _userServices.GetUserFullName(project.OwnerId);

                    projectDtos.Add(new ReadProjectDto
                    {
                        Name = project.Name,
                        Description = project.Description,
                        Creator = creatorFullName,
                        Owner = ownerFullName
                    });
                }

                return new CustomHttpResponse<List<ReadProjectDto>>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = projectDtos,
                    Error = null
                };
            }
            catch (Exception ex)
            {
                return new CustomHttpResponse<List<ReadProjectDto>>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null,
                    Error = ex.Message
                };
            }
        }
    }
}
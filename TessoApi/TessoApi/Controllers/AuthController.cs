using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TessoApi.DTOs.Auth;
using TessoApi.Exceptions;
using TessoApi.Services.Interfaces;

namespace TessoApi.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class AuthController(IAuthenticationService authenticationService) : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService = authenticationService;

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] LogInDto user)
        {
            AuthenticationResponse? response;
            try
            {
                if (user is null || user.UserId is null || user.Password is null)
                {
                    throw new ArgumentNullException("The email and password can not be null."
                        , new Exception(
                            user is null ? $"{nameof(user)} argument is null." 
                            : user.UserId is null ? $"{nameof(user.UserId)} is null." 
                            : $"{nameof(user.Password)} is null.")
                        );
                }
                response = await _authenticationService.Authenticate(user.UserId, user.Password);
            }
            catch (UnauthorizedAccessException ua)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, ua.Message);
            }
            catch (JwtTokenGenerationException jtge)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, jtge.Message);
            }
            catch (ArgumentNullException ane)
            {
                return BadRequest(ane.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong with the request. Please contact support.\n" + ex.Message);
            }
            return Ok(response);
        }

        [HttpPost("logout")]
        public IActionResult LogOut()
        {

            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto user)
        {
            AuthenticationResponse response = new()
            {
                IsAuthenticated = false,
                StatusMessage = "Registration failed. "
            };
            try
            {
                List<ValidationResult> validationResults = [];
                bool isValid = Validator.TryValidateObject(user, new ValidationContext(user, null, null), validationResults, true);

                if (!isValid)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var err in validationResults)
                    {
                        sb.AppendLine($"{err.ErrorMessage}");
                        response.StatusMessage = sb.ToString();
                        response.IsAuthenticated = false;
                        return BadRequest(response);
                    }
                }

                response.IsAuthenticated = await _authenticationService.Register(user);
                if (!response.IsAuthenticated)
                {
                    throw new RegistrationFailedException("Something whent wrong with the registration. Please try again after some time.");
                }
                response.StatusMessage = "Registration Successful.";
                response.Token = Guid.NewGuid().ToString();
                return StatusCode(StatusCodes.Status201Created, response);
            }
            catch (UserAlreadyExistsException uae)
            {
                response.IsAuthenticated = false;
                response.StatusMessage = uae.Message;
                return StatusCode(StatusCodes.Status409Conflict, response);
            }
            catch (RegistrationFailedException rf)
            {
                response.IsAuthenticated = false;
                response.StatusMessage = rf.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            catch (Exception ex)
            {
                response.IsAuthenticated = false;
                response.StatusMessage += ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using TessoApi.DTOs.Auth;
using TessoApi.Helpers.Interfaces;
using TessoApi.Models.Http;
using TessoApi.Services.Interfaces;

namespace TessoApi.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class AuthController(IAuthenticationService authenticationService, IObjectValidationHelper objectValidationHelper) : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService = authenticationService;
        private readonly IObjectValidationHelper _objectValidationHelper = objectValidationHelper;

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] LogInDto user)
        {
            CustomHttpResponse<AuthenticationResponse>? response = await _authenticationService.Authenticate(user);
            return StatusCode((int)response.StatusCode, response.Data);
        }

        [HttpPost("logout")]
        public IActionResult LogOut()
        {
            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto user)
        {
            CustomHttpResponse<AuthenticationResponse> response = await _authenticationService.Register(user);
            return StatusCode((int)response.StatusCode, response.Data);
        }
    }
}

using TessoApi.DTOs.Auth;
using TessoApi.Models.Http;

namespace TessoApi.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<CustomHttpResponse<AuthenticationResponse>> Authenticate(LogInDto user);
        public Task<CustomHttpResponse<AuthenticationResponse>> Register(RegisterDto user);
    }
}

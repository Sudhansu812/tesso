using TessoApi.DTOs.Auth;

namespace TessoApi.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<AuthenticationResponse> Authenticate(string userId, string password);
        public Task<bool> Register(RegisterDto user);
    }
}

using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using TessoApi.Exceptions;
using TessoApi.Models.Identity;
using TessoApi.Services.Interfaces;

namespace TessoApi.Services
{
    public class UserServices(UserManager<User> user) : IUserServices
    {
        private readonly UserManager<User> _user = user;

        public async Task<Guid> GetUserId()
        {
            HttpContextAccessor httpContextAccessor = new HttpContextAccessor();
            string? userId = httpContextAccessor.HttpContext?.User.Identity?.Name;
            if (userId == null)
            {
                throw new JwtTokenExtractionException("JWT token extration failed / unique_name field not found");
            }
            User? result = await _user.FindByNameAsync(userId);
            if (result is null)
            {
                throw new UserNotFoundException("The specified username was not found.");
            }
            return result.Id;
        }

        public async Task<string> GetUserFullName(Guid userId)
        {
            User? user = await _user.FindByIdAsync(userId.ToString());
            if (user is null)
            {
                throw new UserNotFoundException("The specified user was not found.");
            }
            return user.FirstName + " " + user.LastName;
        }
    }
}

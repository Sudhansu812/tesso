using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TessoApi.DTOs.Auth;
using TessoApi.Exceptions;
using TessoApi.Models.Identity;
using TessoApi.Services.Interfaces;

namespace TessoApi.Services
{
    public class AuthenticationService(IConfiguration configuration, UserManager<User> userManager) : IAuthenticationService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly UserManager<User> _userManager = userManager;

        public async Task<AuthenticationResponse> Authenticate(string userId, string password)
        {
            User? user = await _userManager.FindByNameAsync(userId) ?? await _userManager.FindByEmailAsync(userId);
            if (user is null || !await _userManager.CheckPasswordAsync(user, password))
            {
                throw new UnauthorizedAccessException(user == null ? "Invalid User ID." : "Incorrect password.");
            }

            List<string> roles = (List<string>)await _userManager.GetRolesAsync(user);
            List<Claim> claims =
            [
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            ];

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            string? jwtSecret = _configuration["Jwt:Key"];
            if (jwtSecret is null)
            {
                throw new JwtTokenGenerationException("JWT Key not found.");
            }

            SymmetricSecurityKey signInKey = new(Encoding.UTF8.GetBytes(jwtSecret));

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddHours(3),
                claims: claims,
                signingCredentials: new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256)
            );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            if (jwtToken is null)
            {
                throw new JwtTokenGenerationException("JWT token generation failed.");
            }

            return new AuthenticationResponse
            {
                IsAuthenticated = true,
                Token = jwtToken,
                StatusMessage = "Authenticated",
                Expiration = token.ValidTo
            };
            /*
            ExternalLoginInfo? loginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (loginInfo is not null)
            {
                Claim? emailClaim = loginInfo.Principal.Claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Email));
                Claim? userClaim = loginInfo.Principal.Claims.FirstOrDefault(u => u.Type.Equals(ClaimTypes.Name));

                if (emailClaim is not null && userClaim is not null)
                {
                    User user = new User { Email = emailClaim.Value, UserName = userClaim.Value };
                }
            }
            */
        }

        public async Task<bool> Register(RegisterDto registrationDetails)
        {
            if (await _userManager.FindByEmailAsync(registrationDetails.EmailAddress) is not null || await _userManager.FindByNameAsync(registrationDetails.UserName) is not null)
            {
                throw new UserAlreadyExistsException("User already exists.");
            }

            User user = new()
            {
                Email = registrationDetails.EmailAddress,
                UserName = registrationDetails.UserName,
                FirstName = registrationDetails.FirstName,
                LastName = registrationDetails.LastName,
                EmailConfirmed = true
            };

            IdentityResult result = await _userManager.CreateAsync(user, registrationDetails.Password);

            if (!result.Succeeded)
            {
                StringBuilder sb = new();
                foreach (var err in result.Errors)
                {
                    sb.AppendLine(err.Code + ": " + err.Description);
                }
                throw new RegistrationFailedException(sb.ToString());
            }

            return true;
        }
    }
}

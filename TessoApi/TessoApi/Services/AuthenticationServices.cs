using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Net;
using TessoApi.DTOs.Auth;
using TessoApi.Exceptions;
using TessoApi.Models.Http;
using TessoApi.Models.Identity;
using TessoApi.Services.Interfaces;
using TessoApi.Helpers.Interfaces;
using TessoApi.Models.Helper;

namespace TessoApi.Services
{
    public class AuthenticationService(IConfiguration configuration, UserManager<User> userManager, IObjectValidationHelper objectValidationHelper) : IAuthenticationService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly UserManager<User> _userManager = userManager;
        private readonly IObjectValidationHelper _objectValidationHelper = objectValidationHelper;

        public async Task<CustomHttpResponse<AuthenticationResponse>> Authenticate(LogInDto user)
        {
            ObjectValidationResult validationResult = _objectValidationHelper.ValidateObject(user);
            if (!validationResult.IsValid)
            {
                throw new ArgumentNullException(validationResult.Error);
            }

            User? user2 = await _userManager.FindByNameAsync(user.UserId) ?? await _userManager.FindByEmailAsync(user.UserId);
            if (user2 is null || !await _userManager.CheckPasswordAsync(user2, user.Password))
            {
                throw new UnauthorizedAccessException(user2 == null ? "Invalid User ID." : "Incorrect password.");
            }

            List<string> roles = (List<string>)await _userManager.GetRolesAsync(user2);
            List<Claim> claims =
            [
                new Claim(JwtRegisteredClaimNames.UniqueName, user2.UserName),
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

            var response = new AuthenticationResponse
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

            return new CustomHttpResponse<AuthenticationResponse>
            {
                StatusCode = HttpStatusCode.OK,
                Data = response,
                Error = string.Empty
            };
        }

        public async Task<CustomHttpResponse<AuthenticationResponse>> Register(RegisterDto registrationDetails)
        {
            ObjectValidationResult validationResult = _objectValidationHelper.ValidateObject(registrationDetails);
            if (!validationResult.IsValid)
            {
                throw new ArgumentNullException(validationResult.Error);
            }

            if (await _userManager.FindByEmailAsync(registrationDetails.EmailAddress) is not null
                || await _userManager.FindByNameAsync(registrationDetails.UserName) is not null)
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

            return new CustomHttpResponse<AuthenticationResponse>
            {
                StatusCode = HttpStatusCode.Created,
                Data = new AuthenticationResponse { IsAuthenticated = true, StatusMessage = "Registration Successful", Token = Guid.NewGuid().ToString() },
                Error = string.Empty
            };
        }
    }
}

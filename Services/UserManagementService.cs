using ExamNest.DTO.Authentication;
using ExamNest.Enums;
using ExamNest.Errors;
using ExamNest.Interfaces;
using ExamNest.Models;
using ExamNest.Repositories;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ExamNest.Services
{



    public class UserManagementService : IUserManagement
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenManagementService _tokenManagementService;
        private readonly ITrackRepository _trackRepository;

        public UserManagementService(UserManager<User> userManager,
                                     ITokenManagementService tokenManagementService,
                                     ITrackRepository trackRepository)
        {
            this._userManager = userManager;
            this._tokenManagementService = tokenManagementService;
            this._trackRepository = trackRepository;
        }

        public async Task<Tokens> SignInWithEmail(string email, string password)
        {

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Email or Password is not correct.");
            }

            var result = await _userManager.CheckPasswordAsync(user, password);
            if (!result)
            {
                throw new UnauthorizedAccessException("Email or Password is not correct.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (roles == null || roles.Count == 0)
            {
                throw new UnauthorizedAccessException("User has no roles assigned.");
            }

            return await GenerateTokensAsync(user);
        }

        private async Task<Tokens> GenerateTokensAsync(User user)
        {
            var token = _tokenManagementService.GenerateToken(await GetClaims(user));
            var refreshToken = _tokenManagementService.GenerateRefreshToken();

            var tokens = new Tokens()
            {
                AccessToken = token,
                RefreshToken = refreshToken
            };

            return tokens;
        }

        public async Task<Tokens> RegisterUser(string name, string email, string password)
        {
            var username = UsernameGenerator.UsernameGenerator.GenerateUsername();
            var user = new User { Email = email, UserName = username, Name = name };
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                throw new UserCreationErrorException(result.Errors);
            }

            if (!await IsUserInRole(user, Roles.Pending))
            {
                var roleAssignment = await AssignRoleToUser(user, Roles.Pending);
                if (!roleAssignment)
                {
                    throw new UserCreationErrorException("User role assignment failed.");
                }
            }

            return await GenerateTokensAsync(user);

        }

        public async Task<bool> IsUserInRole(User user, Roles role)
        {
            var isInRole = await _userManager.IsInRoleAsync(user, role.ToString());
            return isInRole;
        }

        public async Task<bool> AssignRoleToUser(User user, Roles role)
        {

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Count > 0)
            {
                // Drop the pervious role
                await _userManager.RemoveFromRolesAsync(user, roles);
            }

            var result = await _userManager.AddToRoleAsync(user, role.ToString());


            return result.Succeeded;
        }

        private async Task<List<Claim>> GetClaims(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var roleString = roles.FirstOrDefault() ?? string.Empty;


            var claims = new List<Claim>
                         {
                             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                             new Claim(ClaimTypes.Name, user.UserName),
                             new Claim(ClaimTypes.NameIdentifier, user.Id),
                             new Claim(ClaimTypes.Email, user.Email),
                             new Claim(ClaimTypes.Role, roleString)
                         };


            return claims;
        }

        public async Task<User?> IsUserExistById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<bool> UpgradeUser(UpgradeDTO upgradePayloadDto)
        {
            var user = await _userManager.FindByIdAsync(upgradePayloadDto.UserId);

            if (user == null)
                throw new ResourceNotFoundException("User Not Found");

            if (!await IsUserInRole(user, Roles.Pending))
                throw new InvalidOperationException("Can't regjsgen the role to the user");

            var track = await _trackRepository.GetById(upgradePayloadDto.TrackId);

            if (track == null || (track.BranchID != upgradePayloadDto.BranchId))
            {
                throw new ResourceNotFoundException("Track or Branch not found");
            }

            await AssignRoleToUser(user, upgradePayloadDto.Type);

            return true;
        }

        public Tokens RefreshToken(string refreshToken, string accessToken)
        {

            var isValid = _tokenManagementService.ValidateToken(refreshToken);

            if (!isValid)
            {
                throw new UnauthorizedAccessException("Invalid token");
            }


            var userClaims = _tokenManagementService.GetPrincipalFromExpiredToken(accessToken);

            var newAccessToken = _tokenManagementService.GenerateToken(userClaims.Claims);

            var newRefreshToken = _tokenManagementService.GenerateRefreshToken();

            var tokens = new Tokens()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
            return tokens;

        }

    }
}

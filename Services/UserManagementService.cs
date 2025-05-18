using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ExamNest.Enums;
using ExamNest.Errors;
using ExamNest.Models;
using Microsoft.AspNetCore.Identity;

namespace ExamNest.Services
{


    public class UserManagementService
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly TokenManagementService tokenManagementService;
        public UserManagementService(UserManager<User> _userManager,
            RoleManager<IdentityRole> _roleManager, TokenManagementService _tokenManagementService)
        {
            userManager = _userManager;
            roleManager = _roleManager;
            tokenManagementService = _tokenManagementService;
        }

        public async Task<string> SignInWithEmail(string email, string password)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Email or Password is not correct.");
            }
            var result = await userManager.CheckPasswordAsync(user, password);
            if (!result)
            {
                throw new UnauthorizedAccessException("Email or Password is not correct.");
            }
            var roles = await userManager.GetRolesAsync(user);
            if (roles == null || roles.Count == 0)
            {
                throw new UnauthorizedAccessException("User has no roles assigned.");
            }

            var token = tokenManagementService.GenerateToken(await GetClaims(user));
            return token;
        }

        public async Task<string> RegisterUser(string name, string email, string password)
        {
            var username = UsernameGenerator.UsernameGenerator.GenerateUsername();
            var user = new User { Email = email, UserName = username , Name = name};
            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
               throw new UserCreationErrorException(result.Errors);
            }

            if (!await IsUserInRole(user, Roles.Student))
            {
               var roleAssginment = await AssignRoleToUser(user, Roles.Student);
                if (!roleAssginment)
                {
                    throw new UserCreationErrorException("User role assignment failed.");
                }
            }
           var token = tokenManagementService.GenerateToken(await GetClaims(user));
            return token;
        }

        private async Task<bool> IsUserInRole(User user, Roles role)
        {
            var isInRole = await userManager.IsInRoleAsync(user, role.ToString());
            return isInRole;
        }

        private async Task<bool> AssignRoleToUser(User user, Roles role)
        {
            var result = await userManager.AddToRoleAsync(user, role.ToString());
            

            return result.Succeeded;
        }

        private async Task<List<Claim>> GetClaims(User user)
        {
            var roles = await userManager.GetRolesAsync(user);
            var roleString = roles.FirstOrDefault() ?? string.Empty;
            var role = Enum.TryParse<Roles>(roleString, out var parsedRole) ? parsedRole : Roles.Student;

            IReadOnlyList<string> permissions = role switch
            {
                Roles.Admin => RolePermissions.Admin,
                Roles.Instructor => RolePermissions.Instructor,
                Roles.Student => RolePermissions.Student,
                _ => new List<string>()
            };

            var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, roleString)
                };

            foreach (var permission in permissions)
            {
                claims.Add(new Claim("Permission", permission));
            }

            return claims;
        }



    }
}

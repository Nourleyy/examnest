using ExamNest.Models;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ExamNest.Services
{
    public class UserManagementService
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public UserManagementService(UserManager<User> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            userManager = _userManager;
            roleManager = _roleManager;
        }

        public async Task<User> SignInWithEmail(string email, string password)
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
            return user;
        }

        public async Task<User> RegisterUser(string email, string password)
        {
            var user = new User { Email = email };
            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                throw new Exception("User registration failed.");
            }

            var userRoles = await GetUserRoles(user);
            return user;
        }

        private async Task<IList<string>> GetUserRoles(User user)
        {

            var roles = await userManager.GetRolesAsync(user);

            return roles;

        }

        private Task AssignRolesToUser(User user)
        {
            List<Claim> authClaims = [
                new (ClaimTypes.Email, user.Email),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            ];
            roleManager.CreateAsync("User")
        }
    }
}

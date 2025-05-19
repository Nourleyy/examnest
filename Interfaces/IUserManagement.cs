using ExamNest.DTO.Authentication;
using ExamNest.Enums;
using ExamNest.Models;

namespace ExamNest.Services;

public interface IUserManagement
{
    Task<Tokens> SignInWithEmail(string email, string password);
    Task<Tokens> RegisterUser(string name, string email, string password);
    Task<bool> IsUserInRole(User user, Roles role);
    Task<bool> AssignRoleToUser(User user, Roles role);
    Task<User?> IsUserExistById(string id);
    Task<bool> UpgradeUser(UpgradeDTO upgradePayloadDto);
    Tokens RefreshToken(string refreshToken, string accessToken);
}
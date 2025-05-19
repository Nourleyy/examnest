using System.Security.Claims;

namespace ExamNest.Interfaces;

public interface ITokenManagementService
{
    string GenerateToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    bool ValidateToken(string token);
}
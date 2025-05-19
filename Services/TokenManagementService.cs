using ExamNest.Extensions;
using ExamNest.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ExamNest.Services
{
    public class TokenManagementService : ITokenManagementService
    {
        private readonly IConfiguration configuration;
        private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler = new();

        public TokenManagementService(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        private SecurityTokenDescriptor TokenDescriptor(DateTime expiresIn, IEnumerable<Claim> claims = null)
        {
            var authSigningKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            return new SecurityTokenDescriptor
            {
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"],
                Subject = new ClaimsIdentity(claims),
                Expires = expiresIn,
                SigningCredentials = new SigningCredentials
                           (authSigningKey, SecurityAlgorithms.HmacSha256)
            };
        }
        public string GenerateToken(IEnumerable<Claim> claims)
        {
            var tokenDescriptor = TokenDescriptor(DateTime.Now.AddSeconds(30), claims);
            var token = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
            return jwtSecurityTokenHandler.WriteToken(token);


        }

        public string GenerateRefreshToken()
        {
            var tokenDescriptor = TokenDescriptor(DateTime.Now.AddHours(1));

            var token = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);



            return jwtSecurityTokenHandler.WriteToken(token);


        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = Identity.GetTokenValidationParameters(configuration);
            tokenValidationParameters.ValidateLifetime = false; // here we are saying that we don't care about the token's expiration date
            var principal = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
            return principal;
        }
        public bool ValidateToken(string token)
        {


            try
            {
                jwtSecurityTokenHandler.ValidateToken(token, Identity.GetTokenValidationParameters(configuration), out SecurityToken validatedToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}

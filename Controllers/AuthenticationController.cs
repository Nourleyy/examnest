using ExamNest.DTO.Authentication;
using ExamNest.Filters;
using ExamNest.Repositories;
using ExamNest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly IUserManagement _userManagementService;
        private readonly IAuthenticationRepository authenticationRepository;
        public AuthenticationController(IUserManagement userManagementService, IAuthenticationRepository authenticationRepository)
        {
            this._userManagementService = userManagementService;
            this.authenticationRepository = authenticationRepository;
        }


        [HttpPost("login")]
        [SetJwtCookies]

        public async Task<IActionResult> Login([FromBody] AuthenticationDTO loginDto)
        {
            var token = await _userManagementService.SignInWithEmail(loginDto.Email, loginDto.Password);






            return Ok(token);


        }

        [HttpPost("register")]
        [SetJwtCookies]


        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            var token = await _userManagementService.RegisterUser(registerDto.Name, registerDto.Email, registerDto.Password);
            Response.Cookies.Append("ExamNest.Token", token.AccessToken, ClientCookiesOptions.Options);
            Response.Cookies.Append("ExamNest.Refresh", token.RefreshToken, ClientCookiesOptions.Options);

            return Ok(token);
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            var user = await authenticationRepository.GetCurrentUser();
            return Ok(user);
        }




        [HttpPost("refresh")]
        [SetJwtCookies]

        public IActionResult RefreshToken([FromBody] RefreshTokenDTO refreshTokenDTO)
        {
            var tokens = _userManagementService.RefreshToken(refreshTokenDTO.RefreshToken, refreshTokenDTO.AccessToken);

            return Ok(tokens);
        }
    }
}

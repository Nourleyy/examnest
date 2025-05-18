using System.Security.Claims;
using ExamNest.DTO;
using ExamNest.Enums;
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

        private readonly UserManagementService userManagementService;
        private readonly IStudentRepository studentRepository;
        public AuthenticationController(UserManagementService _userManagementService, IStudentRepository studentRepository)
        {
            userManagementService = _userManagementService;
            this.studentRepository = studentRepository;
        }


        [HttpPost("login")]

        public async Task<IActionResult> Login([FromBody] AuthenticationDTO loginDTO)
        {
            var user = await userManagementService.SignInWithEmail(loginDTO.Email, loginDTO.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            return Ok(user);


        }

        [HttpPost("register")]

        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            var token = await userManagementService.RegisterUser(registerDTO.Name,registerDTO.Email, registerDTO.Password);
            
            return Ok(new { token });
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var CurrentUser = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var user = await studentRepository.GetUserById(CurrentUser);
            if (user == null)
            {
                return Ok("Your Account is Created, but still pending approval.");
            }
            return Ok(user);
        }

      


        //[HttpPost("refresh-token")]
        //public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO refreshTokenDTO)
        //{
        //    var token = await userManagementService.RefreshToken(refreshTokenDTO.Token, refreshTokenDTO.RefreshToken);
        //    if (token == null)
        //    {
        //        return Unauthorized(new { message = "Invalid token." });
        //    }
        //    return Ok(new { token });
        //}
    }
}

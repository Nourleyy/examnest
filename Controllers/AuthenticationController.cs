using ExamNest.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ExamNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {



        public async Task<IActionResult> Login([FromBody] loginDTO loginDTO)
        {
            var user = await userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var result = await userManager.CheckPasswordAsync(user, loginDTO.Password);
            if (result)
            {

            }
            else
            {
                return Unauthorized("Invalid password");
            }


        }
    }
}

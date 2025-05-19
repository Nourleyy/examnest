using ExamNest.Enums;
using ExamNest.Factories;
using ExamNest.Interfaces;
using ExamNest.Repositories;
using ExamNest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class UpgradeDTO
{
    public string UserId { get; set; }
    public Roles Type { get; set; }
    public int TrackId { get; set; }
    public int BranchId { get; set; }

}
namespace ExamNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(Roles.Admin))]

    public class ManagementController : ControllerBase
    {

        private readonly IInstructorRepository instructorRepository;
        private readonly IStudentRepository studentRepository;

        private readonly IUserManagement _userManagementService;
        public ManagementController(IUserManagement userManagementService, IInstructorRepository instructorRepository, IStudentRepository studentRepository)
        {
            _userManagementService = userManagementService;
            this.instructorRepository = instructorRepository;
            this.studentRepository = studentRepository;
        }
        [HttpPost("upgrade")]
        public async Task<IActionResult> UpgradeUserAsync([FromBody] UpgradeDTO upgradePayloadDto)
        {
            switch (upgradePayloadDto.Type)
            {
                case Roles.Instructor:
                    {
                        var existingInstructor = await instructorRepository.GetInstructorByUserId(upgradePayloadDto.UserId);
                        if (existingInstructor != null)
                            return Conflict("User is already an instructor.");

                        await instructorRepository.Create(UserUpgradeFactory.CreateInstructor(upgradePayloadDto.UserId, upgradePayloadDto.TrackId, upgradePayloadDto.BranchId));
                        break;
                    }
                case Roles.Student:
                    {
                        var existingStudent = await studentRepository.GetStudentByUserId(upgradePayloadDto.UserId);
                        if (existingStudent != null)
                            return Conflict("User is already a student.");
                        await studentRepository.Create(UserUpgradeFactory.CreateStudent(upgradePayloadDto.UserId, upgradePayloadDto.TrackId, upgradePayloadDto.BranchId));
                        break;
                    }
            }


            var isUpgraded = await _userManagementService.UpgradeUser(upgradePayloadDto);
            if (!isUpgraded)
            {
                return BadRequest("Somthing went really bad happend");
            }
            return Ok("User upgraded successfully");
        }
    }
}

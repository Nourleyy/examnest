using System.Security.Claims;
using AutoMapper;
using ExamNest.DTO;
using ExamNest.Enums;
using ExamNest.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubmissionsController : ControllerBase
    {

        private readonly ISubmissionRepository submissionRepository;
        private readonly IMapper mapper;
        public SubmissionsController(ISubmissionRepository _submissionRepository, IMapper _mapper)
        {
            submissionRepository = _submissionRepository;
            mapper = _mapper;
        }

        [HttpGet]
        [Authorize(Roles = $"{nameof(Roles.Instructor)},{nameof(Roles.Admin)}")]
        public async Task<IActionResult> GetSubmissions([FromQuery] int page = 1)
        {

            var submissions = await submissionRepository.GetAll(page);

            return Ok(submissions);

        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = $"{nameof(Roles.Student)},{nameof(Roles.Instructor)},{nameof(Roles.Admin)}")]

        public async Task<IActionResult> GetById(int id)
        {
            
            var submission = await submissionRepository.GetById(id);

            

            if (submission == null)
            {
                return NotFound("No Submission Found with this ID");
            }

          

            var currentUser = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (User.IsInRole(nameof(Roles.Student)))
            {
                var studentId = submission.StudentId;
                if (studentId.ToString() != currentUser)
                {
                    return Unauthorized("You are not allowed to access this submission");
                }
            }



            return Ok(mapper.Map<ExamSubmissionView>(submission));
        }

        [HttpGet("{id:int}/details")]
        [Authorize(Roles = $"{nameof(Roles.Instructor)},{nameof(Roles.Admin)}")]

        public async Task<IActionResult> GetSubmissionDetails(int id)
        {
            var details = await submissionRepository.GetSubmissionDetails(id);
            return Ok(details);

        }

        [HttpPost]
        [Authorize(Roles = $"{nameof(Roles.Student)}")]

        public async Task<IActionResult> InsertSubmission(SubmissionInputDTO request)
        {

            // TODO: After Identity We will extract the userId from the token

            var result = await submissionRepository.Create(request);
            return RedirectToAction(nameof(GetById), new { id = result });

        }

        [HttpDelete]
        [Authorize(Roles = $"{nameof(Roles.Admin)}")]

        public async Task<IActionResult> DeleteSubmissionAsync(int id)
        {

            var result = await submissionRepository.Delete(id);
            if (!result)
            {
                return BadRequest("Submission Can't Be Deleted");
            }
            return Ok(result);

        }
    }
}

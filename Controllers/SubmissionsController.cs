using AutoMapper;
using ExamNest.DTO.Exam;
using ExamNest.DTO.Submission;
using ExamNest.Enums;
using ExamNest.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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




            if (!User.IsInRole(nameof(Roles.Student))) return Ok(mapper.Map<ExamSubmissionView>(submission));
            var userId = submission.Student.UserId;

            var currentUser = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId != currentUser)
            {
                return Unauthorized("You are not allowed to access this submission");
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

        public async Task<IActionResult> InsertSubmission(SubmissionPayload request)
        {

            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var studentDto = new SubmssionDTO
            {
                UserId = userId,
                ExamID = request.ExamID,
                Answers = request.Answers
            };


            var result = await submissionRepository.Create(studentDto);
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

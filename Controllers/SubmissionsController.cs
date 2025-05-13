using ExamNest.DTO;
using ExamNest.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExamNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmissionsController : ControllerBase
    {

        private readonly ISubmissionRepository submissionRepository;

        public SubmissionsController(ISubmissionRepository _submissionRepository)
        {
            submissionRepository = _submissionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetSubmissions([FromQuery] int page = 1)
        {

            var submissions = await submissionRepository.GetAll(page);

            return Ok(submissions);

        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var submission = await submissionRepository.GetById(id);

            if (submission == null)
            {
                return NotFound("No Submission Found with this ID");
            }

            return Ok(submission);
        }

        [HttpGet("{id:int}/details")]
        public async Task<IActionResult> GetSubmissionDetails(int id)
        {
            var details = await submissionRepository.GetSubmissionDetails(id);
            return Ok(details);

        }

        [HttpPost]
        public async Task<IActionResult> InsertSubmission(SubmissionInputDTO request)
        {

            // TODO: After Identity We will extract the userId from the token

            var result = await submissionRepository.Create(request);
            return RedirectToAction(nameof(GetById), new { id = result });

        }

        [HttpDelete]
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

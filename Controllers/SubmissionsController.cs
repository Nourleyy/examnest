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

        public SubmissionsController( ISubmissionRepository _submissionRepository)
        {
            submissionRepository = _submissionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetSubmissions()
        {

            var submissions = await submissionRepository.GetAll();
            if (submissions == null || submissions.Count == 0)
            {
                return Ok("No Submissions found");
            }
            return Ok(submissions);

        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var submission = await submissionRepository.GetById(id);
        
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
           
                var result = await submissionRepository.Create(request);
                return Ok(result);
          
        }

        [HttpDelete]
        public IActionResult DeleteSubmission(int id)
        {
            try
            {
                // Delete submission procedure ?
                return Ok();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}

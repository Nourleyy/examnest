using AutoMapper;
using Azure.Core;
using ExamNest.DTO;
using ExamNest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ExamNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmissionsController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;

        public SubmissionsController(AppDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetSubmissions()
        {
            var submissions = _context.ExamSubmissions
                .Include(s => s.Student)
                .ThenInclude(st => st.User)
                .Include(s=> s.Exam)
                .ThenInclude(st => st.Course)
                .ToList();
            return Ok(_mapper.Map<List<SubmissionDTO>>(submissions));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var submission = await _context.GetProcedures().GetStudentExamAnswerDetailsAsync(id);
            if (submission.Count == 0 || submission == null)
            {
                return Ok("Submission Id not found");
            }
            return Ok(submission);
        }

        [HttpGet("{id:int}/details")]
        public async Task<IActionResult> GetSubmissionDetails(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Submission Id is invalid");
            }
            try
            {
                var details = await _context.GetProcedures().GetStudentExamAnswerDetailsAsync(id);
                return Ok(details);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
        [HttpGet("{id:int}/choices")]
        public async Task<IActionResult> GetStudentExamChoiceDetails(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid Submission ID");

            try
            {
                var choices = await _context.GetProcedures().GetStudentExamChoiceDetailsAsync(id);
                return Ok(choices);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertSubmission(SubmissionInputDTO request)
        {
           if(!ModelState.IsValid)
            {
                return BadRequest("Model State");
            }

            try
            {
                var answersJson = JsonConvert.SerializeObject(request.Answers);
                var result = await _context.GetProcedures().SubmitExamAnswersAsync(request.ExamID,request.StudentID, answersJson);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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

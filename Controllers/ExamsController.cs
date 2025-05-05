using AutoMapper;
using ExamNest.DTO;
using ExamNest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamsController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;

        public ExamsController(AppDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetExams()
        {
            var exams = _context.Exams
                .Include(c => c.Course)
                .Select(e => new ExamDTO
                {
                    ExamId = e.ExamId,
                    CourseId = e.CourseId,
                    CourseName = e.Course.CourseName,
                    ExamDate = e.ExamDate
                }).ToList();
            return Ok(_mapper.Map<List<ExamDTO>>(exams));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var exam = await _context.GetProcedures().GetExamDetailsAsync(id);
            if (exam.Count == 0 || exam == null)
            {
                return Ok("Exam Id not found");
            }
            return Ok(exam);
        }

        [HttpGet("{id:int}/display")]
        public async Task<IActionResult> DisplayExam(int id)
        {
            var questions = await _context.GetProcedures().GetExamQuestionListAsync(id);
            if (questions == null || questions.Count == 0)
            {
                return Ok("No Questions found for this Exam");
            }
            var choices = await _context.GetProcedures().GetExamChoiceListAsync(id);
            if (choices == null || choices.Count == 0)
            {
                return Ok("No Choices for this Exam");
            }
            var grouped = questions.Select(q => new QuestionWithChoicesDTO
            {
                QuestionId = q.QuestionID,
                QuestionText = q.QuestionText,
                QuestionType = q.QuestionType,
                Points = q.Points,
                Choices = choices.Where(c => c.QuestionID == q.QuestionID).Select(c => new ChoiceDTO
                {
                    QuestionId = c.QuestionID,
                    ChoiceLetter = c.ChoiceLetter,
                    ChoiceText = c.ChoiceText
                }).ToList()
            }).ToList();

            return Ok(grouped);
        }

        [HttpGet("student-results")]
        public async Task<IActionResult> GetStudentExamResults([FromQuery] int studentId, [FromQuery] int? examId = null)
        {
            if (studentId <= 0)
                return BadRequest("Invalid student ID.");
            try
            {
                var results = await _context.GetProcedures().GetStudentExamResultsAsync(studentId, examId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateExam(ExamInputDTO exam)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var examId = await _context.GetProcedures().CreateExamAndGetIdAsync(exam.CourseId, exam.NoOfQuestions, exam.ExamDate);
                return Ok(examId);

            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }


        [HttpPut]
        public async Task<IActionResult> UpdateExam(int id, DateTime date)
        {
            var exam = await _context.Exams.FindAsync(id);
            if (exam == null)
            {
                return Ok($"No exam foudn with this Id = {id}");
            }

            if (date < DateTime.Today)
            {
                return BadRequest("Exam date cannot be in the past.");
            }

            exam.ExamDate = date;
            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<ExamDTO>(exam));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteExam(int id)
        {
            try
            {
                // Delete exam procedure ?
                return Ok();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}

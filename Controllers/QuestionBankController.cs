using AutoMapper;
using ExamNest.DTO;
using ExamNest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionBankController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;

        public QuestionBankController(AppDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetQuestionBank()
        {
            var Questions = await _context.GetProcedures().GetAllQuestionsAsync();
            return Ok(Questions);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var question = await _context.GetProcedures().GetQuestionByIDAsync(id);
            if (question.Count == 0)
            {
                return Ok();
            }

            return Ok(question);
        }

        [HttpGet("{id:int}/choices")]
        public async Task<IActionResult> GetChoicesByQuestionId(int id)
        {
            var choices = await _context.GetProcedures().GetChoicesByQuestionAsync(id);
            if(choices == null || choices.Count == 0)
            {
                return Ok("No Choices found for this Question");
            }
            var question = await _context.GetProcedures().GetQuestionByIDAsync(id);
            if (question == null || question.Count == 0)
            {
                return Ok("No Question for this Id");
            }
            var grouped = question.Select(q => new QuestionWithChoicesDTO
            {
                QuestionId = id,
                QuestionText = q.QuestionText,
                QuestionType = q.QuestionType,
                Points = q.Points,
                Choices = choices.Select(c => new ChoiceDTO
                {
                    QuestionId = c.QuestionID,
                    ChoiceLetter = c.ChoiceLetter,
                    ChoiceText = c.ChoiceText
                }).ToList()
            }).ToList();


            return Ok(grouped);
        }

        [HttpPost]
        public async Task<IActionResult> InsertQuestion(QuestionBankDTO question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var courseSearch = _context.Courses.FirstOrDefault(c => c.CourseId == question.CourseId);
            if (courseSearch == null)
            {
                return BadRequest("Course Id not found");
            }
            var result = await _context.GetProcedures().CreateQuestionAsync(question.CourseId, question.QuestionText, question.QuestionType,question.ModelAnswer,question.Points);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateQuestion(QuestionBankDTO question, int id)
        {
            var courseSearch = _context.Courses.FirstOrDefault(c => c.CourseId == question.CourseId);
            if (courseSearch == null)
            {
                return BadRequest("Course Id not found");
            }
            var result = await _context.GetProcedures().UpdateQuestionAsync(id, question.CourseId, question.QuestionText, question.QuestionType, question.ModelAnswer, question.Points);
            if (result[0].RowsUpdated == 0)
            {
                return Ok();
            }
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            try
            {
                var result = await _context.GetProcedures().DeleteQuestionAsync(id);
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}

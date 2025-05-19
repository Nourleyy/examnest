using ExamNest.DTO.Question;
using ExamNest.Enums;
using ExamNest.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{nameof(Roles.Instructor)},{nameof(Roles.Admin)}")]

    public class QuestionBankController : ControllerBase
    {


        private readonly IQuestionRepository questionRepository;


        public QuestionBankController(IQuestionRepository _questionRepository)
        {
            questionRepository = _questionRepository;

        }

        [HttpGet]

        public async Task<IActionResult> GetQuestionBank([FromQuery] int page = 1)
        {
            var Questions = await questionRepository.GetAll(page);
            return Ok(Questions);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var question = await questionRepository.GetQuestionById(id);
            if (question == null)
            {
                return NotFound("No Question Found with this ID");
            }

            return Ok(question);
        }

        [HttpGet("{id:int}/choices")]
        public async Task<IActionResult> GetQuestionChoicesByQuestionId(int id)
        {

            var question = await questionRepository.GetQuestionChoicesByQuestionId(id);

            return Ok(question);

        }

        [HttpPost]
        public async Task<IActionResult> InsertQuestion(QuestionBankDTO question)
        {

            var result = await questionRepository.Create(question);

            return RedirectToAction(nameof(GetById), new { id = result });
        }
        [HttpPut]
        public async Task<IActionResult> UpdateQuestion(QuestionBankDTO question, int id)
        {

            var Question = await questionRepository.Update(id, question);

            return Ok(question);

        }

        [HttpDelete]
        [Authorize(Roles = $"{nameof(Roles.Admin)}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var isDeleted = await questionRepository.Delete(id);
            if (isDeleted)
            {
                return Ok("Question Deleted Successfully");
            }

            return BadRequest("Can't Delete this question");
        }
    }
}

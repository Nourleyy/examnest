using ExamNest.DTO;
using ExamNest.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ExamNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionBankController : ControllerBase
    {


        private readonly IQuestionRepository questionRepository;


        public QuestionBankController(IQuestionRepository _questionRepository)
        {
            questionRepository = _questionRepository;

        }

        [HttpGet]
        public async Task<IActionResult> GetQuestionBank()
        {
            var Questions = await questionRepository.GetAll();
            return Ok(Questions);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var question = await questionRepository.GetQuestionById(id);
            if (question.Count == 0)
            {
                return Ok();
            }

            return Ok(question);
        }

        [HttpGet("{id:int}/choices")]
        public async Task<IActionResult> GetQuestionChoicesByQuestionId(int id)
        {

            var question = await questionRepository.GetQuestionChoicesByQuestionId(id);
            if (question.Count == 0)
            {
                return Ok();
            }
            return Ok(question);

        }

        [HttpPost]
        public async Task<IActionResult> InsertQuestion(QuestionBankDTO question)
        {

            var Question = await questionRepository.Create(question);

            return Ok(Question);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateQuestion(QuestionBankDTO question, int id)
        {

            var Question = await questionRepository.Update(id, question);

            return Ok(Question);

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            await questionRepository.Delete(id);
            return Ok();
        }
    }
}

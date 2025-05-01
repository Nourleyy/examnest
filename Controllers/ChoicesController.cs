using AutoMapper;
using ExamNest.DTO;
using ExamNest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChoicesController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;

        public ChoicesController(AppDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var choice = await _context.GetProcedures().GetChoiceByIDAsync(id);
            if (choice.Count == 0)
            {
                return Ok();
            }
            return Ok(choice);
        }

        [HttpPost]
        public async Task<IActionResult> InsertChoice(ChoiceDTO choice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var questionSearch = _context.QuestionBanks.FirstOrDefault(q => q.QuestionId == choice.QuestionId);
            if (questionSearch == null)
            {
                return BadRequest("Question Id not found");
            }
            var result = await _context.GetProcedures().CreateChoiceAsync(choice.QuestionId, choice.ChoiceLetter, choice.ChoiceText);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateChoice(ChoiceDTO choice, int id)
        {
            var questionSearch = _context.QuestionBanks.FirstOrDefault(q => q.QuestionId == choice.QuestionId);
            if (questionSearch == null)
            {
                return BadRequest("Question Id not found");
            }
            var result = await _context.GetProcedures().UpdateChoiceAsync(id, choice.ChoiceLetter, choice.ChoiceText);
            if (result[0].RowsUpdated == 0)
            {
                return Ok();
            }
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteChoice(int id)
        {
            try
            {
                var result = await _context.GetProcedures().DeleteChoiceAsync(id);
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}

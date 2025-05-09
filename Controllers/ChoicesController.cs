using ExamNest.DTO;
using ExamNest.Interfaces;
using ExamNest.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExamNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChoicesController : ControllerBase
    {
        private readonly IChoiceRepository choiceRepository;

        public ChoicesController(AppDBContext context, IChoiceRepository _choiceRepository)
        {
            choiceRepository = _choiceRepository;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var choice = await choiceRepository.GetById(id);
            if (choice == null)
            {
                return Ok();
            }
            return Ok(choice);
        }

        [HttpPost]

        public async Task<IActionResult> InsertChoice(ChoiceDTO choice)
        {
            var result = await choiceRepository.Create(choice);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateChoice(ChoiceDTO choice, int id)
        {
            var result = await choiceRepository.Update(choice, id);
            return result ? Ok() : BadRequest("Update failed");

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteChoice(int id)
        {
            try
            {
                var result = await choiceRepository.Delete(id);
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}

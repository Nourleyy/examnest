using ExamNest.DTO;
using ExamNest.Enums;
using ExamNest.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{nameof(Roles.Instructor)},{nameof(Roles.Admin)}")]

    public class ChoicesController : ControllerBase
    {
        private readonly IChoiceRepository choiceRepository;

        public ChoicesController(IChoiceRepository _choiceRepository)
        {
            choiceRepository = _choiceRepository;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var choice = await choiceRepository.GetById(id);
            if (choice == null)
            {
                return NotFound("No Choice with the provided ID");
            }

            return Ok(choice);
        }

        [HttpPost]

        public async Task<IActionResult> InsertChoice(ChoiceDTO choice)
        {
            var result = await choiceRepository.Create(choice);
            if (result == null)
            {
                return BadRequest("Choice can't be created");
            }

            return RedirectToAction(nameof(GetById), new { id = result });
        }
        [HttpPut]
        public async Task<IActionResult> UpdateChoice(ChoiceDTO choice, int id)
        {
            var result = await choiceRepository.Update(id, choice);

            if (result == null)
            {
                return BadRequest("Error while updating the choice");
            }

            return Ok(choice);

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteChoice(int id)
        {

            var result = await choiceRepository.Delete(id);
            if (!result)
            {
                return BadRequest("Choice Can't Be Deleted");
            }
            return Ok("Choice Deleted Successfully ");

        }
    }
}

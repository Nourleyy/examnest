using ExamNest.DTO;
using ExamNest.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ExamNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorsController : ControllerBase
    {
        public readonly IInstructorRepository InstructorRepository;

        public InstructorsController(IInstructorRepository _instructorRepository)
        {
            InstructorRepository = _instructorRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetInstructorsAsync([FromQuery] int page = 1)
        {
            var instructors = await InstructorRepository.GetAll(page);
            return Ok(instructors);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var instructor = await InstructorRepository.GetById(id);

            if (instructor == null)
            {
                return NotFound("No Instructor with this ID");
            }

            return Ok(instructor);
        }

        [HttpPost]
        public async Task<IActionResult> InsertInstructor(UserDTO instructor)
        {

            var Inserted = await InstructorRepository.Create(instructor);

            return Ok(Inserted);

        }
        [HttpPut]
        public async Task<IActionResult> UpdateInstructor(UserDTO instructor, int id)
        {
            var updated = await InstructorRepository.Update(id, instructor);

            return Ok(updated);

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteInstructor(int id)
        {

            var deleted = await InstructorRepository.Delete(id);
            if (!deleted)
            {
                return BadRequest("Instructor Can't Be Deleted");
            }
            return Ok("Instructor Deleted Successfully");


        }
    }
}

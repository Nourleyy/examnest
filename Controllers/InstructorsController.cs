using ExamNest.DTO;
using ExamNest.Models;
using ExamNest.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ExamNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorsController : ControllerBase
    {
        public readonly IInstructorRepository InstructorRepository;

        public InstructorsController(AppDBContext context, IInstructorRepository _instructorRepository)
        {
            InstructorRepository = _instructorRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetInstructorsAsync()
        {
            var instructors = await InstructorRepository.GetAll();
            return Ok(instructors);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var instructors = await InstructorRepository.GetById(id);

            return Ok(instructors);
        }

        [HttpPost]
        public async Task<IActionResult> InsertInstructor(UserDTO instructor)
        {

            var Inserted = await InstructorRepository.Create(instructor);
            if (Inserted == null)
            {
                return BadRequest("Track Id or Branch Id not found");
            }
            return Ok(Inserted);

        }
        [HttpPut]
        public async Task<IActionResult> UpdateInstructor(UserDTO instructor, int id)
        {
            var updated = await InstructorRepository.Update(id, instructor);
            if (updated == null)
            {
                return BadRequest("Track Id or Branch Id not found");
            }
            return Ok(updated);

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteInstructor(int id)
        {

            var deleted = await InstructorRepository.Delete(id);
            return Ok();


        }
    }
}

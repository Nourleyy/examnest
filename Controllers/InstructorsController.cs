using AutoMapper;
using ExamNest.DTO.Student;
using ExamNest.Enums;
using ExamNest.Models;
using ExamNest.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class InstructorsController : ControllerBase
    {
        public readonly IInstructorRepository InstructorRepository;
        private readonly IMapper mapper;

        public InstructorsController(IInstructorRepository _instructorRepository, IMapper mapper)
        {
            InstructorRepository = _instructorRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = $"{nameof(Roles.Instructor)},{nameof(Roles.Admin)}")]
        public async Task<IActionResult> GetInstructorsAsync([FromQuery] int page = 1)
        {
            var instructors = await InstructorRepository.GetAll(page);
            return Ok(instructors);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = $"{nameof(Roles.Student)},{nameof(Roles.Instructor)},{nameof(Roles.Admin)}")]
        public async Task<IActionResult> GetById(int id)
        {
            var instructor = await InstructorRepository.GetById(id);

            if (instructor == null)
            {
                return NotFound("No Instructor with this ID");
            }

            return Ok(instructor);
        }

        [HttpPut]
        [Authorize(Roles = $"{nameof(Roles.Instructor)},{nameof(Roles.Admin)}")]
        public async Task<IActionResult> UpdateInstructor(UpdateDto instructorPayload, int id)
        {
            var isExisted = await InstructorRepository.GetById(id);
            if (isExisted == null)
            {
                return NotFound("No Instructor with this ID");
            }
            var instructor = mapper.Map<Instructor>(instructorPayload);
            var updated = await InstructorRepository.Update(id, instructor);

            return Ok(instructor);

        }

        [HttpDelete]
        [Authorize(Roles = $"{nameof(Roles.Admin)}")]
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

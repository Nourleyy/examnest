using ExamNest.DTO;
using ExamNest.Enums;
using ExamNest.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{nameof(Roles.Student)},{nameof(Roles.Instructor)},{nameof(Roles.Admin)}")]

    public class CoursesController : ControllerBase
    {
        private readonly ICoursesRepository coursesRepository;

        public CoursesController(ICoursesRepository _courseRepository)
        {
            coursesRepository = _courseRepository;
        }

        [HttpGet]

        public async Task<IActionResult> GetAll([FromQuery] int page = 1)
        {
            var courses = await coursesRepository.GetAll(page);
            return Ok(courses);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await coursesRepository.GetById(id);

            return Ok(course);
        }

        [HttpPost]
        [Authorize(Roles = $"{nameof(Roles.Instructor)},{nameof(Roles.Admin)}")]

        public async Task<IActionResult> InsertCourse(CourseDTO course)
        {

            var result = await coursesRepository.Create(course);
            return RedirectToAction(nameof(GetById), new { id = result });
        }
        [HttpPut]
        [Authorize(Roles = $"{nameof(Roles.Instructor)},{nameof(Roles.Admin)}")]

        public async Task<IActionResult> UpdateCourse(CourseDTO course, int id)
        {
            var result = await coursesRepository.Update(id, course);
            return Ok(course);
        }

        [HttpDelete]
        [Authorize(Roles = $"{nameof(Roles.Admin)}")]

        public async Task<IActionResult> DeleteCourse(int id)
        {

            var result = await coursesRepository.Delete(id);
            if (!result)
            {
                return BadRequest("Course Can't Be Deleted");
            }
            return Ok(result);

        }
    }
}

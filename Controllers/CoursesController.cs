using ExamNest.DTO;
using ExamNest.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExamNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICoursesRepository coursesRepository;

        public CoursesController(ICoursesRepository _courseRepository)
        {
            coursesRepository = _courseRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var courses = await coursesRepository.GetAll();
            return Ok(courses);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var tracks = await coursesRepository.GetById(id);

            return Ok(tracks);
        }

        [HttpPost]
        public async Task<IActionResult> InsertCourse(CourseDTO course)
        {

            var result = await coursesRepository.Create(course);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCourse(CourseDTO course, int id)
        {
            var result = await coursesRepository.Update(id, course);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
                var result = await coursesRepository.Delete(id);
                return result ? Ok() : BadRequest("Course not deleted");
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}

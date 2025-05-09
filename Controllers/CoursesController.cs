using ExamNest.DTO;
using ExamNest.Interfaces;
using ExamNest.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ExamNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICoursesRepository coursesRepository;
        private readonly ITrackRepository trackRepository;

        public CoursesController(ICoursesRepository _courseRepository, ITrackRepository _trackRepository)
        {
            coursesRepository = _courseRepository;
            trackRepository = _trackRepository;
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

            return tracks == null ? NotFound() : Ok(tracks);
        }

        [HttpPost]
        public async Task<IActionResult> InsertCourse(CourseDTO course)
        {

            var trackSearch = await trackRepository.GetById(course.TrackId);
            if (trackSearch == null)
            {
                return BadRequest("Track Id not found");
            }
            var result = await coursesRepository.Create(course);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCourse(CourseDTO course, int id)
        {
            var trackSearch = await trackRepository.GetById(course.TrackId);
            if (trackSearch == null)
            {
                return BadRequest("Track Id not found");
            }
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

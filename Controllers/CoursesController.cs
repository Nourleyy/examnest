using ExamNest.DTO;
using ExamNest.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExamNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly AppDBContext _context;

        public CoursesController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTracks()
        {
            var courses = await _context.GetProcedures().GetAllCoursesAsync();
            return Ok(courses);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var tracks = await _context.GetProcedures().GetCourseByIDAsync(id);
            if (tracks.Count == 0)
            {
                return NotFound();
            }

            return Ok(tracks);
        }

        [HttpPost]
        public async Task<IActionResult> InsertCourse(CourseDTO course)
        {

            var trackSearch = _context.Tracks.FirstOrDefault(t => t.TrackId == course.TrackId);
            if (trackSearch == null)
            {
                return BadRequest("Track Id not found");
            }
            var result = await _context.GetProcedures().CreateCourseAsync(course.TrackId, course.CourseName);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCourse(CourseDTO course, int id)
        {
            var trackSearch = _context.Tracks.FirstOrDefault(c => c.TrackId == course.TrackId);
            if (trackSearch == null)
            {
                return BadRequest("Track Id not found");
            }
            var result = await _context.GetProcedures().UpdateCourseAsync(id, course.TrackId, course.CourseName);
            if (result[0].RowsUpdated == 0)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
                var result = await _context.GetProcedures().DeleteCourseAsync(id);
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}

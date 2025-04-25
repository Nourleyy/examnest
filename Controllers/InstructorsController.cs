using AutoMapper;
using ExamNest.DTO;
using ExamNest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorsController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;

        public InstructorsController(AppDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetInstructors()
        {
            var instructors = _context.Instructors
                .Include(t => t.Track)
                .Include(b => b.Branch)
                .Include(u => u.User)
                .ToList();
            return Ok(_mapper.Map<List<InstructorViewDTO>>(instructors));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var instructors = await _context.GetProcedures().GetInstructorByIDAsync(id);
            if (instructors.Count == 0)
            {
                return NotFound();
            }

            return Ok(instructors);
        }

        [HttpPost]
        public async Task<IActionResult> InsertInstructor(InstructorDTO instructor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var trackSearch = _context.Tracks.FirstOrDefault(t => t.TrackId == instructor.TrackId);
            if (trackSearch == null)
            {
                return BadRequest("Track Id not found");
            }
            var branchSearch = _context.Branches.FirstOrDefault(b => b.BranchId == instructor.BranchId);
            if (branchSearch == null)
            {
                return BadRequest("Branch Id not found");
            }
            var UserSearch = _context.Users.FirstOrDefault(u => u.Id == instructor.UserId);
            if (UserSearch == null)
            {
                return BadRequest("User Id not found");
            }
            var result = await _context.GetProcedures().CreateInstructorAsync(instructor.BranchId, instructor.TrackId, instructor.UserId);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateInstructor(InstructorDTO instructor, int id)
        {
            var trackSearch = _context.Tracks.FirstOrDefault(t => t.TrackId == instructor.TrackId);
            if (trackSearch == null)
            {
                return BadRequest("Track Id not found");
            }
            var branchSearch = _context.Branches.FirstOrDefault(b => b.BranchId == instructor.BranchId);
            if (branchSearch == null)
            {
                return BadRequest("Branch Id not found");
            }
            var UserSearch = _context.Users.FirstOrDefault(u => u.Id == instructor.UserId);
            if (UserSearch == null)
            {
                return BadRequest("User Id not found");
            }
            var result = await _context.GetProcedures().UpdateInstructorAsync(id, instructor.BranchId, instructor.TrackId, instructor.UserId);
            if (result[0].RowsUpdated == 0)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteInstructor(int id)
        {
            try
            {
                var result = await _context.GetProcedures().DeleteInstructorAsync(id);
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}

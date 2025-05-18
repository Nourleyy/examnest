using AutoMapper;
using ExamNest.DTO;
using ExamNest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;

        public StudentsController(AppDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            var students = _context.Students
                .Include(t => t.Track)
                .Include(b => b.Branch)
                .ToList();
            return Ok(_mapper.Map<List<UserViewDTO>>(students));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var students = await _context.GetProcedures().GetStudentByIDAsync(id);
            if (students.Count == 0)
            {
                return NotFound();
            }

            return Ok(students);
        }

        [HttpPost]
        public async Task<IActionResult> InsertStudent(UserDTO Student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var trackSearch = _context.Tracks.FirstOrDefault(t => t.TrackId == Student.TrackId);
            if (trackSearch == null)
            {
                return BadRequest("Track Id not found");
            }
            var branchSearch = _context.Branches.FirstOrDefault(b => b.BranchId == Student.BranchId);
            if (branchSearch == null)
            {
                return BadRequest("Branch Id not found");
            }
            //var UserSearch = _context.Users.FirstOrDefault(u => u.Id == Student.UserId);
            //if (UserSearch == null)
            //{
            //    return BadRequest("User Id not found");
            //}
            var result = await _context.GetProcedures().CreateStudentAsync(Student.BranchId, Student.TrackId, "1");
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateStudent(UserDTO Student, int id)
        {
            var trackSearch = _context.Tracks.FirstOrDefault(t => t.TrackId == Student.TrackId);
            if (trackSearch == null)
            {
                return BadRequest("Track Id not found");
            }
            var branchSearch = _context.Branches.FirstOrDefault(b => b.BranchId == Student.BranchId);
            if (branchSearch == null)
            {
                return BadRequest("Branch Id not found");
            }
            //var UserSearch = _context.Users.FirstOrDefault(u => u.Id == Student.UserId);
            //if (UserSearch == null)
            //{
            //    return BadRequest("User Id not found");
            //}
            var result = await _context.GetProcedures().UpdateStudentAsync(id, Student.BranchId, Student.TrackId, Student.UserId);
            if (result[0].RowsUpdated == 0)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                // missing stored procedure
                return Ok();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}

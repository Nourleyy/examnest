using AutoMapper;
using ExamNest.DTO.Student;
using ExamNest.Enums;
using ExamNest.Interfaces;
using ExamNest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize($"{nameof(Roles.Admin)},{nameof(Roles.Instructor)}")]

    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository studentRepository;
        private readonly IMapper _mapper;

        public StudentsController(IMapper mapper, IStudentRepository studentRepository)
        {
            _mapper = mapper;
            this.studentRepository = studentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents([FromQuery] int page = 1)
        {

            var students = await studentRepository.GetAll(page);

            return Ok(students);
        }

        [HttpGet("{id:int}")]

        public async Task<IActionResult> GetById(int id)
        {
            var student = await studentRepository.GetById(id);


            return Ok(student);
        }

        [HttpPut]

        public async Task<IActionResult> UpdateStudent(UpdateDto Student, int id)
        {

            var isExisted = await studentRepository.GetById(id);
            if (isExisted == null)
            {
                return NotFound("No Student with this ID");
            }

            var updated = await studentRepository.Update(id, _mapper.Map<Student>(Student));



            return RedirectToAction(nameof(GetById), new { id = updated });
        }

        [HttpDelete]
        [Authorize(Roles = $"{nameof(Roles.Admin)}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {

            var isExisted = await studentRepository.GetById(id);
            if (isExisted == null)
            {
                return NotFound("No Student with this ID");
            }
            var deleted = await studentRepository.Delete(id);
            return Ok(deleted);

        }
    }
}

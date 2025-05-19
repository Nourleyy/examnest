using AutoMapper;
using ExamNest.DTO.Exam;
using ExamNest.Enums;
using ExamNest.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class ExamsController : ControllerBase
    {
        private readonly IExamRepository _examRepository;
        private readonly IMapper _mapper;


        public ExamsController(IExamRepository examRepository, IMapper mapper)
        {

            this._mapper = mapper;
            this._examRepository = examRepository;

        }

        [HttpGet]
        [Authorize(Roles = $"{nameof(Roles.Instructor)},{nameof(Roles.Admin)}")]
        public async Task<IActionResult> GetExamsAsync([FromQuery] int page = 1)
        {
            var exams = await _examRepository.GetExams(page);
            return Ok(exams);

        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = $"{nameof(Roles.Student)},{nameof(Roles.Instructor)},{nameof(Roles.Admin)}")]
        public async Task<IActionResult> GetById(int id)
        {

            var exam = await _examRepository.GetExamDetailsById(id);



            return Ok(exam);
        }

        [HttpGet("{id:int}/display")]
        [Authorize(Roles = $"{nameof(Roles.Student)},{nameof(Roles.Instructor)},{nameof(Roles.Admin)}")]
        public async Task<IActionResult> DisplayExam(int id)
        {
            var exam = await _examRepository.GetExam(id);
            if (exam == null)
            {
                return NotFound("No Exam by ID");
            }
            return Ok(exam);


        }

        [HttpGet("student-results")]
        [Authorize(Roles = $"{nameof(Roles.Instructor)},{nameof(Roles.Admin)}")]
        public async Task<IActionResult> GetStudentExamResults([FromQuery] int studentId, int examId)
        {

            var result = await _examRepository.GetExamResultByStudentId(studentId, examId);

            if (result == null)
            {
                NotFound("There's no Results for the provided data");
            }

            return Ok(result);


        }

        [HttpPost]
        [Authorize(Roles = $"{nameof(Roles.Instructor)},{nameof(Roles.Admin)}")]
        public async Task<IActionResult> CreateExam(ExamCreatePayload examPayloadDto)
        {


            var examDto = _mapper.Map<ExamDTO>(examPayloadDto);
            var exam = await _examRepository.Create(examDto);
            return RedirectToAction(nameof(GetById), new { id = exam });

        }


        [HttpPut]
        [Authorize(Roles = $"{nameof(Roles.Instructor)},{nameof(Roles.Admin)}")]
        public async Task<IActionResult> UpdateExam([FromQuery] ExamUpdatePayloadDTO examUpdatePayload)
        {


            var examUpdate = _mapper.Map<ExamDTO>(examUpdatePayload);
            var updatedExam = await _examRepository.Update(examUpdatePayload.Id, examUpdate);
            return Ok(examUpdatePayload);
        }

        [HttpDelete]
        [Authorize(Roles = $"{nameof(Roles.Admin)}")]
        public async Task<IActionResult> DeleteExam([FromQuery] int id)
        {

            var deleted = await _examRepository.Delete(id);

            if (deleted)
            {
                return Ok("Exam Deleted Successfully");
            }
            return BadRequest("Exam can't be deleted");
        }
    }
}

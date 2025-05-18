using AutoMapper;
using ExamNest.DTO;
using ExamNest.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExamNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamsController : ControllerBase
    {
        private readonly IExamRepository examRepository;
        private readonly IMapper mapper;


        public ExamsController(IExamRepository _examRepository, IMapper _mapper)
        {

            mapper = _mapper;
            examRepository = _examRepository;

        }

        [HttpGet]
        public async Task<IActionResult> GetExamsAsync([FromQuery] int page = 1)
        {
            var exams = await examRepository.GetExams(page);
            return Ok(exams);

        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {

            var exam = await examRepository.GetExamDetailsById(id);
            return Ok(exam);
        }

        [HttpGet("{id:int}/display")]
        public async Task<IActionResult> DisplayExam(int id)
        {
            var exam = await examRepository.GetExam(id);
            if (exam == null)
            {
                return NotFound("No Exam by ID");
            }
            return Ok(exam);


        }

        [HttpGet("student-results")]
        public async Task<IActionResult> GetStudentExamResults([FromQuery] int studentId, [FromQuery] int? examId = null)
        {

            var results = await examRepository.GetExamResultByStudentId(studentId, examId.Value);

            return Ok(results);


        }

        [HttpPost]
        public async Task<IActionResult> CreateExam(ExamInputDTO examPayloadDto)
        {


            var examPayload = mapper.Map<ExamDTO>(examPayloadDto);
            var exam = await examRepository.Create(examPayload);
            return RedirectToAction(nameof(GetById), new { id = exam });

        }


        [HttpPut]
        public async Task<IActionResult> UpdateExam([FromQuery] ExamUpdatePayloadDTO examUpdatePayload)
        {


            var examUpdate = mapper.Map<ExamDTO>(examUpdatePayload);
            var updatedExam = await examRepository.Update(examUpdatePayload.Id, examUpdate);
            return Ok(examUpdatePayload);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteExam([FromQuery] int id)
        {

            var deleted = await examRepository.Delete(id);

            if (deleted)
            {
                return Ok("Exam Deleted Successfully");
            }
            return BadRequest("Exam can't be deleted");
        }
    }
}

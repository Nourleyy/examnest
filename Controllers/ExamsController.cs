using AutoMapper;
using ExamNest.DTO;
using ExamNest.Repositories;
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
        public async Task<IActionResult> GetExamsAsync()
        {
            var exams = await examRepository.GetExams();
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
            return Ok();

            //var questions = await _context.GetProcedures().GetExamQuestionListAsync(id);
            //if (questions == null || questions.Count == 0)
            //{
            //    return Ok("No Questions found for this Exam");
            //}
            //var choices = await _context.GetProcedures().GetExamChoiceListAsync(id);
            //if (choices == null || choices.Count == 0)
            //{
            //    return Ok("No Choices for this Exam");
            //}
            //var grouped = questions.Select(q => new QuestionWithChoicesDTO
            //{
            //    QuestionId = q.QuestionID,
            //    QuestionText = q.QuestionText,
            //    QuestionType = q.QuestionType,
            //    Points = q.Points,
            //    Choices = choices.Where(c => c.QuestionID == q.QuestionID).Select(c => new ChoiceDTO
            //    {
            //        QuestionId = c.QuestionID,
            //        ChoiceLetter = c.ChoiceLetter,
            //        ChoiceText = c.ChoiceText
            //    }).ToList()
            //}).ToList();

            //return Ok(grouped);
        }

        [HttpGet("student-results")]
        public async Task<IActionResult> GetStudentExamResults([FromQuery] int studentId, [FromQuery] int? examId = null)
        {
            return Ok();

            //if (studentId <= 0)
            //    return BadRequest("Invalid student ID.");
            //try
            //{
            //    var results = await _context.GetProcedures().GetStudentExamResultsAsync(studentId, examId);
            //    return Ok(results);
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest(ex.Message);
            //}
        }

        [HttpPost]
        public async Task<IActionResult> CreateExam(ExamInputDTO examPayloadDto)
        {


            var exam = mapper.Map<ExamDTO>(examPayloadDto);
            var examId = await examRepository.Create(exam);
            return Ok(examId);

        }


        [HttpPut]
        public async Task<IActionResult> UpdateExam([FromQuery] ExamUpdatePayloadDTO examUpdatePayload)
        {

            var examUpdate = mapper.Map<ExamDTO>(examUpdatePayload);
            var updatedExam = await examRepository.Update(examUpdatePayload.Id, examUpdate);
            return Ok(updatedExam);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteExam(int id)
        {
            try
            {
                // Delete exam procedure ?
                return Ok();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}

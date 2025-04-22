using ExamNest.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExamNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly AppDBContext appDBContext;
        public TestController(AppDBContext _app)
        {

            appDBContext = _app;
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            // Example of using the DbContext
            var exam_results = await appDBContext.GetProcedures().GetStudentExamResultsAsync(1, 1);
            return Ok(exam_results);
        }

    }
}

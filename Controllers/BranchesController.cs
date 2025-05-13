using ExamNest.DTO;
using ExamNest.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExamNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchesController : ControllerBase
    {
        private readonly IBranchRepository branchRepository;
        public BranchesController(IBranchRepository _branch)
        {
            branchRepository = _branch;
        }

        [HttpGet]
        public async Task<IActionResult> GetBranches([FromQuery] int page = 1)
        {
            var branches = await branchRepository.GetAll(page);

            return Ok(branches);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var branch = await branchRepository.GetById(id);

            if (branch == null)
            {
                return NotFound("No Branch Found with this ID");
            }

            return Ok(branch);

        }

        [HttpPost]
        public async Task<IActionResult> InsertBranch(BranchDTO branch)
        {

            var result = await branchRepository.Create(branch);
            if (result == null)
            {
                return BadRequest("Branch not created");
            }

            return RedirectToAction(nameof(GetById), new { id = 1 });
        }
        [HttpPut]
        public async Task<IActionResult> UpdateBranch(BranchDTO branch, int id)
        {
            var result = await branchRepository.Update(id, branch);

            if (result == null)
            {
                return BadRequest("Error while updating the branch");
            }



            return Ok(result);

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBranch(int id)
        {

            var isDeleted = await branchRepository.Delete(id);

            if (isDeleted)
            {
                return Ok("Branch Deleted Successfully");
            }
            return BadRequest("Branch couldn't be deleted");


        }


    }
}

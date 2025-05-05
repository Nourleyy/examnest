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
        public async Task<IActionResult> GetBranches()
        {
            var branches = await branchRepository.GetAll();
            return Ok(branches);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var branches = await branchRepository.GetById(id);
            if (!branches.Any())
            {
                return NotFound();
            }

            return Ok(branches);
        }

        [HttpPost]
        public async Task<IActionResult> InsertBranch(BranchDTO branch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await branchRepository.Insert(branch.BranchName);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateBranch(BranchDTO branch, int id)
        {
            var result = await branchRepository.Update(id, branch.BranchName);

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            try
            {
                var result = await branchRepository.Delete(id);
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

    }
}

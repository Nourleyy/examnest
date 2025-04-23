using AutoMapper;
using ExamNest.DTO;
using ExamNest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ExamNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchesController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;

        public BranchesController(AppDBContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetBranches()
        {
            var branches = await _context.GetProcedures().GetAllBranchesAsync();
            return Ok(branches);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var branches = await _context.GetProcedures().GetBranchByIDAsync(id);
            if(branches.Count == 0)
            {
                return NotFound();
            }
            
            return Ok(branches);
        }

        [HttpPost]
        public async Task<IActionResult> InsertBranch(BranchDTO branch)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _context.GetProcedures().CreateBranchAsync(branch.BranchName);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateBranch(BranchDTO branch, int id)
        {
            var result  = await _context.GetProcedures().UpdateBranchAsync(id,branch.BranchName);
            if (result[0].RowsUpdated == 0)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            try
            {
                var result = await _context.GetProcedures().DeleteBranchAsync(id);
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
        
    }
}

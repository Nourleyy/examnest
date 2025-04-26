using System.Diagnostics;
using AutoMapper;
using ExamNest.DTO;
using ExamNest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TracksController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;

        public TracksController(AppDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetTracks()
        {
            var tracks = await _context.GetProcedures().GetAllTracksAsync();
            return Ok(tracks);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var tracks = await _context.GetProcedures().GetTrackByIDAsync(id);
            if (tracks.Count == 0)
            {
                return NotFound();
            }

            return Ok(tracks);
        }

        [HttpPost]
        public async Task<IActionResult> InsertTrack(TrackDTO track)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var branchSearch = _context.Branches.FirstOrDefault(b => b.BranchId == track.BranchId);
            if (branchSearch == null)
            {
                return BadRequest("Branch Id not found");
            }
            var result = await _context.GetProcedures().CreateTrackAsync(track.BranchId,track.TrackName);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateBranch(TrackDTO track, int id)
        {
            var branchSearch = _context.Branches.FirstOrDefault(b => b.BranchId == track.BranchId);
            if (branchSearch == null)
            {
                return BadRequest("Branch Id not found");
            }
            var result = await _context.GetProcedures().UpdateTrackAsync(id,track.BranchId, track.TrackName);
            if (result[0].RowsUpdated == 0)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTrack(int id)
        {
            try
            {
                var result = await _context.GetProcedures().DeleteTrackAsync(id);
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}

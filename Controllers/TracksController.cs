using ExamNest.DTO;
using ExamNest.Interfaces;
using ExamNest.Models;
using ExamNest.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ExamNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TracksController : ControllerBase
    {
        private readonly IBranchRepository branchRepository;
        private readonly ITrackRepository trackRepository;

        public TracksController(AppDBContext context, IBranchRepository _branchRepository, ITrackRepository _trackRepository)
        {
            branchRepository = _branchRepository;
            trackRepository = _trackRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetTracks()
        {
            var tracks = await trackRepository.GetAll();
            return Ok(tracks);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var tracks = await trackRepository.GetById(id);

            return Ok(tracks);
        }

        [HttpPost]
        public async Task<IActionResult> InsertTrack(TrackDTO track)
        {

            var branchSearch = await branchRepository.GetById(track.BranchId);
            if (branchSearch == null)
            {
                return BadRequest("Branch Id not found");
            }
            var result = await trackRepository.Create(track);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateBranch(TrackDTO track, int id)
        {
            var branchSearch = await branchRepository.GetById(track.BranchId);
            if (branchSearch == null)
            {
                return BadRequest("Branch Id not found");
            }
            var result = await trackRepository.Update(id, track);

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTrack(int id)
        {
            try
            {
                var result = await trackRepository.Delete(id);
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}

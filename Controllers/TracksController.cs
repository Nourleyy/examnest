using ExamNest.DTO.Track;
using ExamNest.Enums;
using ExamNest.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{nameof(Roles.Student)},{nameof(Roles.Instructor)},{nameof(Roles.Admin)}")]
    public class TracksController : ControllerBase
    {
        private readonly ITrackRepository trackRepository;

        public TracksController(ITrackRepository _trackRepository)
        {
            trackRepository = _trackRepository;
        }

        [HttpGet]

        public async Task<IActionResult> GetTracks([FromQuery] int page = 1)
        {
            var tracks = await trackRepository.GetAll(page);
            return Ok(tracks);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var track = await trackRepository.GetById(id);

            if (track == null)
            {
                return NotFound("No Track Found with this ID");
            }




            return Ok(track);
        }

        [HttpPost]
        [Authorize(Roles = $"{nameof(Roles.Instructor)},{nameof(Roles.Admin)}")]
        public async Task<IActionResult> InsertTrack(TrackDTO track)
        {
            var result = await trackRepository.Create(track);
            return RedirectToAction(nameof(GetById), new { id = result });

        }
        [HttpPut]
        [Authorize(Roles = $"{nameof(Roles.Instructor)},{nameof(Roles.Admin)}")]
        public async Task<IActionResult> UpdateBranch(TrackDTO track, int id)
        {

            var result = await trackRepository.Update(id, track);

            return Ok(track);
        }

        [HttpDelete]
        [Authorize(Roles = $"{nameof(Roles.Admin)}")]
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

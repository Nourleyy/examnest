using ExamNest.DTO.Track;
using ExamNest.Interfaces;
using ExamNest.Models;

namespace ExamNest.Repositories
{


    public interface ITrackRepository : IGeneric<TrackDTO>
    {
        Task<IEnumerable<GetAllTracksResult>> GetAll(int page);
        Task<GetTrackByIDResult?> GetById(int id);
    }




}

using ExamNest.DTO;
using ExamNest.Interfaces;
using ExamNest.Models;

namespace ExamNest.Repositories
{


    public interface ITrackRepository : IGeneric<TrackDTO>
    {
        Task<IEnumerable<GetAllTracksResult>> GetAll();
        Task<GetTrackByIDResult?> GetById(int id);
    }




}

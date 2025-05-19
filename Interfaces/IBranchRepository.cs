using ExamNest.DTO.Track;
using ExamNest.Models;

namespace ExamNest.Interfaces
{
    public interface IBranchRepository : IGeneric<BranchDTO>
    {

        Task<IEnumerable<GetAllBranchesResult>> GetAll(int page);
        Task<GetBranchByIDResult?> GetById(int id);

    }
}

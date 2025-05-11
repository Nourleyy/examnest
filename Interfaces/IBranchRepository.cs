using ExamNest.DTO;
using ExamNest.Models;

namespace ExamNest.Interfaces
{
    public interface IBranchRepository : IGeneric<BranchDTO>
    {

        Task<IEnumerable<GetAllBranchesResult>> GetAll(int skip = 0, int page = 1);
        Task<GetBranchByIDResult?> GetById(int id);

    }
}

using ExamNest.DTO;
using ExamNest.Models;

namespace ExamNest.Interfaces
{
    public interface IBranchRepository : IGeneric<BranchDTO>
    {

        Task<IEnumerable<GetAllBranchesResult>> GetAll(int page = 1);
        Task<GetBranchByIDResult?> GetById(int id);

    }
}

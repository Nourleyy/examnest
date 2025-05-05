using ExamNest.Models;

namespace ExamNest.Interfaces
{
    public interface IBranchRepository
    {
        Task<bool> Delete(int id);
        Task<IEnumerable<GetAllBranchesResult>> GetAll();
        Task<IEnumerable<GetBranchByIDResult>> GetById(int id);
        Task<IEnumerable<CreateBranchResult>> Insert(string branchName);
        Task<bool> Update(int id, string branchName);
    }
}

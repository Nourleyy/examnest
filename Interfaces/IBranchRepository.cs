using ExamNest.Models;

namespace ExamNest.Interfaces
{
    public interface IBranchRepository
    {
        Task<IEnumerable<DeleteBranchResult>> Delete(int id);
        Task<IEnumerable<GetAllBranchesResult>> GetAll();
        Task<IEnumerable<GetBranchByIDResult>> GetById(int id);
        Task<IEnumerable<CreateBranchResult>> Insert(string branchName);
        Task<IEnumerable<UpdateBranchResult>> Update(int id, string branchName);
    }
}

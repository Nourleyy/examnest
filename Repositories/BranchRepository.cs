using ExamNest.Interfaces;
using ExamNest.Models;

namespace ExamNest.Repositories
{

    public class BranchRepository : GenericRepository, IBranchRepository
    {
        public BranchRepository(AppDBContext appDB) : base(appDB)
        {
        }

        public async Task<IEnumerable<GetAllBranchesResult>> GetAll()
        {
            return await appDBContextProcedures.GetAllBranchesAsync();
        }
        public async Task<IEnumerable<GetBranchByIDResult>> GetById(int id)
        {
            return await appDBContextProcedures.GetBranchByIDAsync(id);
        }
        public async Task<IEnumerable<UpdateBranchResult>> Update(int id, string branchName)
        {
            return await appDBContextProcedures.UpdateBranchAsync(id, branchName);
        }
        public async Task<IEnumerable<CreateBranchResult>> Insert(string branchName)
        {
            return await appDBContextProcedures.CreateBranchAsync(branchName);
        }
        public async Task<IEnumerable<DeleteBranchResult>> Delete(int id)
        {
            return await appDBContextProcedures.DeleteBranchAsync(id);
        }

    }
}

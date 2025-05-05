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
        public async Task<bool> Update(int id, string branchName)
        {
            var Updated = await appDBContextProcedures.UpdateBranchAsync(id, branchName);
            return Updated[0].RowsUpdated > 0;
        }
        public async Task<IEnumerable<CreateBranchResult>> Insert(string branchName)
        {
            return await appDBContextProcedures.CreateBranchAsync(branchName);
        }
        public async Task<bool> Delete(int id)
        {
            var Deleted = await appDBContextProcedures.DeleteBranchAsync(id);
            return Deleted[0].RowsDeleted > 0;
        }

    }
}

using ExamNest.DTO;
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
        public async Task<GetBranchByIDResult?> GetById(int id)
        {
            var branch = await appDBContextProcedures.GetBranchByIDAsync(id);
            return branch.FirstOrDefault();
        }

        public async Task<bool> Delete(int id)
        {
            var Deleted = await appDBContextProcedures.DeleteBranchAsync(id);
            return Deleted[0].RowsDeleted > 0;
        }


        public async Task<BranchDTO?> Create(BranchDTO examDto)
        {
            var result = await appDBContextProcedures.CreateBranchAsync(examDto.BranchName);

            return result.Count > 0 ? examDto : null;

        }

        public async Task<BranchDTO?> Update(int id, BranchDTO entity)
        {
            var Updated = await appDBContextProcedures.UpdateBranchAsync(id, entity.BranchName);


            return Updated.Count > 0 ? entity : null;
        }

    }
}

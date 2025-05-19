using ExamNest.DTO.Track;
using ExamNest.Errors;
using ExamNest.Interfaces;
using ExamNest.Models;

namespace ExamNest.Repositories
{

    public class BranchRepository : GenericRepository, IBranchRepository
    {
        public BranchRepository(AppDBContext appDB) : base(appDB)
        {
        }

        public async Task<IEnumerable<GetAllBranchesResult>> GetAll(int page)
        {
            var pages = await AppDbContextProcedures.GetAllBranchesAsync();

            var paginatedResult = pages.Skip(CalculatePagination(page)).Take(LimitPerPage);

            return paginatedResult;

        }
        public async Task<GetBranchByIDResult?> GetById(int id)
        {
            var branchList = await AppDbContextProcedures.GetBranchByIDAsync(id);

            return branchList.FirstOrDefault();

        }


        public async Task<bool> Delete(int id)
        {
            var branch = await GetById(id);

            if (branch == null)
            {
                throw new ResourceNotFoundException("Branch not found to be deleted");
            }

            var resultList = await AppDbContextProcedures.DeleteBranchAsync(id);


            return resultList.FirstOrDefault()?.RowsDeleted > 0;
        }



        public async Task<decimal?> Create(BranchDTO branchDto)
        {
            var result = await AppDbContextProcedures.CreateBranchAsync(branchDto.BranchName);



            return result.FirstOrDefault()?.BranchID;

        }

        public async Task<int?> Update(int id, BranchDTO branchDto)
        {
            var branch = await GetById(id);

            if (branch == null)
            {
                throw new ResourceNotFoundException("Branch not found to be updated");
            }

            var updated = await AppDbContextProcedures.UpdateBranchAsync(id, branchDto.BranchName);




            return updated.FirstOrDefault()?.RowsUpdated > 0 ? id : null;
        }

    }
}

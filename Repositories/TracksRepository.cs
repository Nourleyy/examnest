using ExamNest.DTO;
using ExamNest.Errors;
using ExamNest.Interfaces;
using ExamNest.Models;

namespace ExamNest.Repositories
{
    class TracksRepository : GenericRepository, ITrackRepository
    {

        private readonly IBranchRepository branchRepository;

        public TracksRepository(AppDBContext appDB, IBranchRepository _branchRepository) : base(appDB)
        {
            branchRepository = _branchRepository;
        }

        public async Task<decimal?> Create(TrackDTO trackDto)
        {
            var branchSearch = await branchRepository.GetById(trackDto.BranchId);
            if (branchSearch == null)
            {
                throw new ResourceNotFoundException("Branch not found");
            }
            var Created = await appDBContextProcedures.CreateTrackAsync(trackDto.BranchId, trackDto.TrackName);
            return Created.FirstOrDefault()?.TrackID;
        }

        public async Task<bool> Delete(int id)
        {
            var track = await GetById(id);
            if (track == null)
            {
                throw new ResourceNotFoundException("Track not found to be deleted");
            }
            var Deleted = await appDBContextProcedures.DeleteTrackAsync(id);
            return Deleted.FirstOrDefault().RowsDeleted > 0;


        }

        public async Task<IEnumerable<GetAllTracksResult>> GetAll(int page)
        {
            IEnumerable<GetAllTracksResult> trackList = await appDBContextProcedures.GetAllTracksAsync();
            var paginatedResult = trackList.Skip(CalculatePagination(page)).Take(LimitPerPage);
            return paginatedResult;
        }

        public async Task<GetTrackByIDResult?> GetById(int id)
        {
            var trackList = await appDBContextProcedures.GetTrackByIDAsync(id);
            return trackList.FirstOrDefault();
        }

        public async Task<int?> Update(int id, TrackDTO entity)
        {
            var branchSearch = await branchRepository.GetById(entity.BranchId);
            if (branchSearch == null)
            {
                throw new ResourceNotFoundException("Branch not found");
            }
            var update = await appDBContextProcedures.UpdateTrackAsync(id, entity.BranchId, entity.TrackName);
            return id;
        }


    }
}

using ExamNest.DTO;
using ExamNest.Models;

namespace ExamNest.Repositories
{
    class TracksRepository : GenericRepository, ITrackRepository
    {

        public TracksRepository(AppDBContext appDB) : base(appDB)
        {
        }

        public async Task<TrackDTO?> Create(TrackDTO entity)
        {
            var Created = await appDBContextProcedures.CreateTrackAsync(entity.BranchId, entity.TrackName);
            return Created.Count > 0 ? entity : null;
        }

        public async Task<bool> Delete(int id)
        {

            var Deleted = await appDBContextProcedures.DeleteTrackAsync(id);
            return Deleted.Count > 0;


        }

        public async Task<IEnumerable<GetAllTracksResult>> GetAll()
        {
            IEnumerable<GetAllTracksResult> trackList = await appDBContextProcedures.GetAllTracksAsync();
            return trackList;
        }

        public async Task<GetTrackByIDResult> GetById(int id)
        {
            var trackList = await appDBContextProcedures.GetTrackByIDAsync(id);
            return trackList[0];
        }

        public async Task<TrackDTO?> Update(int id, TrackDTO entity)
        {
            var Updated = await appDBContextProcedures.UpdateTrackAsync(id, entity.BranchId, entity.TrackName);
            return Updated.Count > 0 ? entity : null;
        }


    }
}

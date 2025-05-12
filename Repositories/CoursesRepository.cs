using ExamNest.DTO;
using ExamNest.Interfaces;
using ExamNest.Models;

namespace ExamNest.Repositories
{
    public class CoursesRepository : GenericRepository, ICoursesRepository
    {
        private readonly ITrackRepository trackRepository;

        public CoursesRepository(AppDBContext appDB, ITrackRepository _trackRepository) : base(appDB)
        {
            trackRepository = _trackRepository;
        }

        public async Task<IEnumerable<GetAllCoursesResult>> GetAll(int page)
        {
            var results = await appDBContextProcedures.GetAllCoursesAsync();
            var paginatedResults = results.Skip(CalculatePagination(page)).Take(LimitPerPage);
            return paginatedResults;
        }

        public async Task<GetCourseByIDResult?> GetById(int id)
        {
            var result = await appDBContextProcedures.GetCourseByIDAsync(id);


            return result.FirstOrDefault();

        }

        public async Task<CourseDTO?> Update(int id, CourseDTO course)
        {
            var trackSearch = await trackRepository.GetById(course.TrackId);
            if (trackSearch == null)
            {
                return null;
            }
            var result = await appDBContextProcedures.UpdateCourseAsync(id, course.TrackId, course.CourseName);

            return result.Count > 0 ? course : null;
        }

        public async Task<CourseDTO?> Create(CourseDTO course)
        {
            var trackSearch = await trackRepository.GetById(course.TrackId);
            if (trackSearch == null)
            {
                return null;
            }
            var result = await appDBContextProcedures.CreateCourseAsync(course.TrackId, course.CourseName);
            return result.Count > 0 ? course : null;
        }

        public async Task<bool> Delete(int id)
        {
            var result = await appDBContextProcedures.DeleteCourseAsync(id);
            return result.Count > 0;
        }


    }


}

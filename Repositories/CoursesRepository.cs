using ExamNest.DTO.Course;
using ExamNest.Errors;
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
            var results = await AppDbContextProcedures.GetAllCoursesAsync();
            var paginatedResults = results.Skip(CalculatePagination(page)).Take(LimitPerPage);
            return paginatedResults;
        }

        public async Task<GetCourseByIDResult?> GetById(int id)
        {
            var result = await AppDbContextProcedures.GetCourseByIDAsync(id);


            return result.FirstOrDefault();

        }

        public async Task<int?> Update(int id, CourseDTO course)
        {
            var trackSearch = await trackRepository.GetById(course.TrackId);
            if (trackSearch == null)
            {
                throw new ResourceNotFoundException("Provided track ID is not found");
            }

            var courseById = await GetById(id);
            if (courseById == null)
            {
                throw new ResourceNotFoundException("Provided Course ID is not found");
            }



            var result = await AppDbContextProcedures.UpdateCourseAsync(id, course.TrackId, course.CourseName);

            return result.FirstOrDefault()?.RowsUpdated > 0 ? id : null;
        }

        public async Task<decimal?> Create(CourseDTO course)
        {
            var trackSearch = await trackRepository.GetById(course.TrackId);
            if (trackSearch == null)
            {
                throw new ResourceNotFoundException("Provided track ID is not found");
            }
            var result = await AppDbContextProcedures.CreateCourseAsync(course.TrackId, course.CourseName);
            return result.FirstOrDefault()?.CourseID;
        }

        public async Task<bool> Delete(int id)
        {
            var course = await GetById(id);
            if (course == null)
            {
                throw new ResourceNotFoundException("Course not found to be deleted");
            }
            var result = await AppDbContextProcedures.DeleteCourseAsync(id);
            return result.FirstOrDefault()?.RowsDeleted > 0;


        }


    }


}

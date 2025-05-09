using ExamNest.DTO;
using ExamNest.Interfaces;
using ExamNest.Models;

namespace ExamNest.Repositories
{
    public class CoursesRepository : GenericRepository, ICoursesRepository
    {
        public CoursesRepository(AppDBContext appDB) : base(appDB)
        {
        }

        public async Task<List<GetAllCoursesResult>> GetAll()
        {
            return await appDBContextProcedures.GetAllCoursesAsync();
        }

        public async Task<GetCourseByIDResult?> GetById(int id)
        {
            var result = await appDBContextProcedures.GetCourseByIDAsync(id);


            return result.FirstOrDefault();

        }

        public async Task<CourseDTO?> Update(int id, CourseDTO course)
        {
            var result = await appDBContextProcedures.UpdateCourseAsync(id, course.TrackId, course.CourseName);

            return result[0].RowsUpdated > 0 ? course : null;
        }

        public async Task<CourseDTO?> Create(CourseDTO course)
        {
            var result = await appDBContextProcedures.CreateCourseAsync(course.TrackId, course.CourseName);
            return result.Count > 0 ? course : null;
        }

        public async Task<bool> Delete(int id)
        {
            var result = await appDBContextProcedures.DeleteCourseAsync(id);
            return result[0].RowsDeleted > 0;
        }


    }


}

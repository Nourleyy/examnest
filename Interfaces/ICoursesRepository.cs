using ExamNest.DTO.Course;
using ExamNest.Models;

namespace ExamNest.Interfaces
{
    public interface ICoursesRepository : IGeneric<CourseDTO>
    {
        Task<IEnumerable<GetAllCoursesResult>> GetAll(int page);
        Task<GetCourseByIDResult?> GetById(int id);
    }
}
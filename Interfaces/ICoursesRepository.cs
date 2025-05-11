using ExamNest.DTO;
using ExamNest.Models;

namespace ExamNest.Interfaces
{
    public interface ICoursesRepository : IGeneric<CourseDTO>
    {
        Task<List<GetAllCoursesResult>> GetAll();
        Task<GetCourseByIDResult?> GetById(int id);
    }
}
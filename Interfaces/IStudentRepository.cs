using ExamNest.DTO.Authentication;
using ExamNest.Models;

namespace ExamNest.Interfaces;

public interface IStudentRepository : IGeneric<Student>
{
    Task<StudentViewDto?> GetStudentByUserId(string userId);
    Task<List<StudentViewDto>> GetAll(int page);
    Task<StudentViewDto> GetById(int id);

}
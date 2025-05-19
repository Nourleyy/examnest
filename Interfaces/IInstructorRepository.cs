using ExamNest.DTO.Authentication;
using ExamNest.Interfaces;
using ExamNest.Models;

namespace ExamNest.Repositories
{


    public interface IInstructorRepository : IGeneric<Instructor>
    {
        Task<IEnumerable<InstructorViewDto>> GetAll(int page);
        Task<GetInstructorByIDResult?> GetById(int id);
        Task<InstructorViewDto?> GetInstructorByUserId(string userId);
    }




}

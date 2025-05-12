using ExamNest.DTO;
using ExamNest.Interfaces;
using ExamNest.Models;

namespace ExamNest.Repositories
{


    public interface IInstructorRepository : IGeneric<UserDTO>
    {
        Task<IEnumerable<UserViewDTO>> GetAll(int page);
        Task<GetInstructorByIDResult?> GetById(int id);
    }




}

using ExamNest.DTO.Question;
using ExamNest.Models;

namespace ExamNest.Interfaces
{
    public interface IChoiceRepository : IGeneric<ChoiceDTO>
    {
        Task<GetChoiceByIDResult?> GetById(int id);
    }
}
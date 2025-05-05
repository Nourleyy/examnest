using ExamNest.DTO;
using ExamNest.Models;

namespace ExamNest.Interfaces
{
    public interface IChoiceRepository
    {
        Task<bool> Create(ChoiceDTO choice);
        Task<bool> Delete(int id);
        Task<GetChoiceByIDResult?> GetById(int id);
        Task<bool> Update(ChoiceDTO choice, int id);
    }
}
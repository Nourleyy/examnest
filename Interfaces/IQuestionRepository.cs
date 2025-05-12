using ExamNest.DTO;
using ExamNest.Models;

namespace ExamNest.Interfaces
{
    public interface IQuestionRepository : IGeneric<QuestionBankDTO>
    {
        Task<IEnumerable<GetAllQuestionsResult>> GetAll(int page);
        Task<GetQuestionByIDResult?> GetQuestionById(int id);
        Task<QuestionWithChoicesDTO> GetQuestionChoicesByQuestionId(int id);
    }
}

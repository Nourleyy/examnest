using ExamNest.DTO;
using ExamNest.Models;

namespace ExamNest.Interfaces
{
    public interface IQuestionRepository : IGeneric<QuestionBankDTO>
    {
        Task<IEnumerable<GetAllQuestionsResult>> GetAll(int page);
        Task<List<GetQuestionByIDResult>> GetQuestionById(int id);
        Task<List<QuestionWithChoicesDTO>> GetQuestionChoicesByQuestionId(int id);
    }
}

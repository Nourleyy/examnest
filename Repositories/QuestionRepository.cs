using ExamNest.DTO;
using ExamNest.Interfaces;
using ExamNest.Models;

namespace ExamNest.Repositories
{
    public interface IQuestionRepository : IGeneric<QuestionBankDTO>
    {
        Task<List<QuestionBankDTO>> GetAll();
        Task<List<QuestionBankDTO>> GetQuestionById(int id);
        Task<List<ChoiceDTO>> GetChoicesByQuestionId(int id);
    }
    public class QuestionRepository : GenericRepository, IQuestionRepository
    {
        public QuestionRepository(AppDBContext appDB) : base(appDB)
        {
        }

        public Task<QuestionBankDTO?> Create(QuestionBankDTO examDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<QuestionBankDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<List<ChoiceDTO>> GetChoicesByQuestionId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<QuestionBankDTO>> GetQuestionById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<QuestionBankDTO?> Update(int id, QuestionBankDTO entity)
        {
            throw new NotImplementedException();
        }
    }
}

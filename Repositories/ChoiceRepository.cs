using ExamNest.DTO;
using ExamNest.Errors;
using ExamNest.Interfaces;
using ExamNest.Models;

namespace ExamNest.Repositories
{
    public class ChoiceRepository : GenericRepository, IChoiceRepository
    {
        private readonly IQuestionRepository questionRepository;

        public ChoiceRepository(AppDBContext appDB, IQuestionRepository questionRepository) : base(appDB)
        {
            this.questionRepository = questionRepository;
        }
        public async Task<GetChoiceByIDResult?> GetById(int id)
        {
            var ChoiceList = await appDBContextProcedures.GetChoiceByIDAsync(id);

            return ChoiceList.FirstOrDefault();
        }
        public async Task<decimal?> Create(ChoiceDTO choice)
        {
            var question = await questionRepository.GetQuestionById(choice.QuestionId);
            if (question == null)
            {
                throw new ResourceNotFoundException("Question not found to create a choice for it!");
            }
            var created = await appDBContextProcedures.CreateChoiceAsync(choice.QuestionId, choice.ChoiceLetter, choice.ChoiceText);

            return created.FirstOrDefault()?.ChoiceID;
        }

        public async Task<int?> Update(int id, ChoiceDTO choice)
        {
            var question = await questionRepository.GetQuestionById(choice.QuestionId);

            if (question == null)
            {
                throw new ResourceNotFoundException("Question not found to update a choice for it!");
            }

            var isChoiceExists = await GetById(id);
            if (isChoiceExists == null)
            {
                throw new ResourceNotFoundException("No Choice Found to be Updated!");

            }
            var updated = await appDBContextProcedures.UpdateChoiceAsync(id, choice.ChoiceLetter, choice.ChoiceText);
            return updated.FirstOrDefault()?.RowsUpdated > 0 ? id : null;
        }

        public async Task<bool> Delete(int id)
        {
            var choice = await GetById(id);
            if (choice == null)
            {
                throw new ResourceNotFoundException("Choice not found to be deleted");
            }
            var deleted = await appDBContextProcedures.DeleteChoiceAsync(id);

            return deleted.FirstOrDefault()?.RowsDeleted > 0;
        }

    }


}

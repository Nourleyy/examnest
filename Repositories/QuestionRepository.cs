using ExamNest.DTO;
using ExamNest.Errors;
using ExamNest.Interfaces;
using ExamNest.Models;

namespace ExamNest.Repositories
{

    public class QuestionRepository : GenericRepository, IQuestionRepository
    {
        private readonly ICoursesRepository coursesRepository;
        public QuestionRepository(
            AppDBContext appDB,
            ICoursesRepository _coursesRepository
            ) : base(appDB)
        {
            coursesRepository = _coursesRepository;
        }

        public async Task<QuestionBankDTO?> Create(QuestionBankDTO question)
        {
            var courseSearch = await coursesRepository.GetById(question.CourseId);
            if (courseSearch == null)
            {
                throw new ResourceNotFoundException("Course not found");
            }
            var result = await appDBContextProcedures.CreateQuestionAsync(question.CourseId, question.QuestionText, question.QuestionType, question.ModelAnswer, question.Points);

            return result.Count > 0 ? question : null;
        }

        public async Task<bool> Delete(int id)
        {
            var question = await GetQuestionById(id);
            if (question == null)
            {
                throw new ResourceNotFoundException("Question not found to be deleted");
            }
            var result = await appDBContextProcedures.DeleteQuestionAsync(id);
            return result.FirstOrDefault().RowsDeleted > 0;

        }

        public async Task<IEnumerable<GetAllQuestionsResult>> GetAll(int page)
        {
            var result = await appDBContextProcedures.GetAllQuestionsAsync();
            var paginatedResult = result.Skip(CalculatePagination(page)).Take(LimitPerPage);
            return paginatedResult;

        }

        public async Task<QuestionWithChoicesDTO> GetQuestionChoicesByQuestionId(int id)
        {
            var question = await GetQuestionById(id);
            if (question == null)
                throw new ResourceNotFoundException("Question not found");

            var choices = await appDBContextProcedures.GetChoicesByQuestionAsync(id);


            var result = new QuestionWithChoicesDTO
            {
                QuestionId = question.QuestionID,
                QuestionText = question.QuestionText,
                QuestionType = question.QuestionType,
                Points = question.Points,
                Choices = choices.Select(c => new ChoiceDTO
                {
                    QuestionId = c.QuestionID,
                    ChoiceLetter = c.ChoiceLetter,
                    ChoiceText = c.ChoiceText
                }).ToList()
            };

            return result;
        }

        public async Task<GetQuestionByIDResult?> GetQuestionById(int id)
        {
            var result = await appDBContextProcedures.GetQuestionByIDAsync(id);
            return result.FirstOrDefault();
        }

        public async Task<QuestionBankDTO?> Update(int id, QuestionBankDTO question)
        {

            var courseSearch = await coursesRepository.GetById(question.CourseId);
            if (courseSearch == null)
            {
                throw new ResourceNotFoundException("Course not found");
            }
            var result = await appDBContextProcedures.UpdateQuestionAsync(id, question.CourseId, question.QuestionText, question.QuestionType, question.ModelAnswer, question.Points);




            return result.Count == 0 ? null : question;
        }
    }
}

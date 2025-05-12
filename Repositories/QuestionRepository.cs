using ExamNest.DTO;
using ExamNest.Interfaces;
using ExamNest.Models;

namespace ExamNest.Repositories
{

    public class QuestionRepository : GenericRepository, IQuestionRepository
    {
        private readonly ICoursesRepository coursesRepository;
        public QuestionRepository(
            AppDBContext appDB,
            ICoursesRepository _coursesRepository) : base(appDB)
        {
            coursesRepository = _coursesRepository;
        }

        public async Task<QuestionBankDTO?> Create(QuestionBankDTO question)
        {
            var courseSearch = await coursesRepository.GetById(question.CourseId);
            if (courseSearch == null)
            {
                throw new Exception("Course not found");
            }
            var result = await appDBContextProcedures.CreateQuestionAsync(question.CourseId, question.QuestionText, question.QuestionType, question.ModelAnswer, question.Points);

            return result.Count > 0 ? question : null;
        }

        public async Task<bool> Delete(int id)
        {
            var result = await appDBContextProcedures.DeleteQuestionAsync(id);
            return result.Count > 0;

        }

        public async Task<IEnumerable<GetAllQuestionsResult>> GetAll(int page)
        {
            var result = await appDBContextProcedures.GetAllQuestionsAsync();
            var paginatedResult = result.Skip(CalculatePagination(page)).Take(LimitPerPage);
            return paginatedResult;

        }

        public async Task<List<QuestionWithChoicesDTO>> GetQuestionChoicesByQuestionId(int id)
        {
            var choices = await appDBContextProcedures.GetChoicesByQuestionAsync(id);

            var question = await GetQuestionById(id);

            var grouped = question.Select(q => new QuestionWithChoicesDTO
            {
                QuestionId = id,
                QuestionText = q.QuestionText,
                QuestionType = q.QuestionType,
                Points = q.Points,
                Choices = choices.Select(c => new ChoiceDTO
                {
                    QuestionId = c.QuestionID,
                    ChoiceLetter = c.ChoiceLetter,
                    ChoiceText = c.ChoiceText
                }).ToList()
            }).ToList();


            return grouped;
        }

        public async Task<List<GetQuestionByIDResult>> GetQuestionById(int id)
        {
            var result = await appDBContextProcedures.GetQuestionByIDAsync(id);
            return result;
        }

        public async Task<QuestionBankDTO?> Update(int id, QuestionBankDTO question)
        {

            var courseSearch = await coursesRepository.GetById(question.CourseId);
            if (courseSearch == null)
            {
                throw new Exception("Course not found");
            }
            var result = await appDBContextProcedures.UpdateQuestionAsync(id, question.CourseId, question.QuestionText, question.QuestionType, question.ModelAnswer, question.Points);




            return result.Count == 0 ? null : question;
        }
    }
}

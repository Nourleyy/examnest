using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ExamNest.DTO;
using ExamNest.Interfaces;
using ExamNest.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamNest.Repositories
{
    public interface IExamRepository : IGeneric<ExamDTO>
    {
        public Task<GetExamDetailsResult?> GetExamDetailsById(int id);
        public Task<IEnumerable<ExamDTO?>> GetExams();
        public Task<ExamDTO?> GetExamById(int id);

        public Task<GetStudentExamResultsResult?> GetExamResultByStudentId(int studentId, int examId);

        public Task<List<QuestionWithChoicesDTO>> GetExam(int id);


    }
    public class ExamRepository : GenericRepository, IExamRepository
    {
        private readonly IMapper mapper;
        public ExamRepository(AppDBContext appDB, IMapper _mapper) : base(appDB)
        {
            mapper = _mapper;
        }

        public async Task<ExamDTO?> Create(ExamDTO examDto)
        {
            var exam = await appDBContextProcedures.CreateExamAndGetIdAsync(examDto.CourseId, examDto.NoOfQuestions, examDto.ExamDate);
            return exam.Count > 0 ? mapper.Map<ExamDTO>(exam) : null;

        }

        public async Task<IEnumerable<ExamDTO?>> GetExams()
        {
            var exams = await _appDBContext.Exams
                .Include(c => c.Course)
                .Select(e => new ExamDTO
                {
                    ExamId = e.ExamId,
                    CourseId = e.CourseId,
                    CourseName = e.Course.CourseName,
                    ExamDate = e.ExamDate,
                    NoOfQuestions = e.Questions.Count
                }).ToListAsync();
            return exams;
        }

        public async Task<GetExamDetailsResult?> GetExamDetailsById(int id)
        {
            var exam = await appDBContextProcedures.GetExamDetailsAsync(id);
            return exam.FirstOrDefault();
        }

        public async Task<ExamDTO?> GetExamById(int id)
        {
            var exam = await _appDBContext.Exams
                .Include(c => c.Course)
                .Include(q => q.Questions)
                .Select(e => new ExamDTO
                {
                    ExamId = e.ExamId,
                    CourseId = e.CourseId,
                    CourseName = e.Course.CourseName,
                    ExamDate = e.ExamDate,
                    NoOfQuestions = e.Questions.Count
                })
                .Where(e => e.ExamId == id)
                .FirstOrDefaultAsync();

            if (exam == null)
            {
                return null;
            }


                return mapper.Map<ExamDTO>(exam);
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ExamDTO?> Update(int id, ExamDTO entity)
        {
            var exam = await GetExamById(id);

            if (exam == null)
            {
                return null;
            }

            exam.ExamDate = entity.ExamDate;

            await _appDBContext.SaveChangesAsync();

            return exam;


        }

        public async Task<GetStudentExamResultsResult?> GetExamResultByStudentId(int studentId, int examId)
        {
            var result = await appDBContextProcedures.GetStudentExamResultsAsync(studentId, examId);

            return result.FirstOrDefault();
        }

      public async Task<List<QuestionWithChoicesDTO>> GetExam(int id)
        {

            var questions = await appDBContextProcedures.GetExamQuestionListAsync(id);
            if (questions == null || questions.Count == 0)
            {
                throw new Exception("Exam has No Questions");
            }
            var choices = await appDBContextProcedures.GetExamChoiceListAsync(id);
            if (choices == null || choices.Count == 0)
            {
                throw new Exception("No Choices for this Exam");
            }
            var exam =  questions.Select(q => new QuestionWithChoicesDTO
             {
                 QuestionId = q.QuestionID,
                 QuestionText = q.QuestionText,
                 QuestionType = q.QuestionType,
                 Points = q.Points,
                 Choices = choices.Where(c => c.QuestionID == q.QuestionID).Select(c => new ChoiceDTO
                 {
                     QuestionId = c.QuestionID,
                     ChoiceLetter = c.ChoiceLetter,
                     ChoiceText = c.ChoiceText
                 }).ToList()
             }).ToList(); 
            
            
            return exam;
        }
    }
}

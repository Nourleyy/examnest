using AutoMapper;
using ExamNest.DTO;
using ExamNest.Errors;
using ExamNest.Interfaces;
using ExamNest.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamNest.Repositories
{
    public class ExamRepository : GenericRepository, IExamRepository
    {
        private readonly IMapper mapper;
        private readonly ICoursesRepository coursesRepository;
        public ExamRepository(AppDBContext appDB, IMapper _mapper, ICoursesRepository coursesRepository) : base(appDB)
        {
            mapper = _mapper;
            this.coursesRepository = coursesRepository;
        }

        public async Task<decimal?> Create(ExamDTO examDto)
        {
            var courseSearch = await coursesRepository.GetById(examDto.CourseId);
            if (courseSearch == null)
            {
                throw new ResourceNotFoundException("Course not found");
            }
            var exam = await appDBContextProcedures.CreateExamAndGetIdAsync(examDto.CourseId, examDto.NoOfQuestions, examDto.ExamDate);

            if (exam.FirstOrDefault() == null)
            {
                throw new InvalidOperationException("Exam not created");
            }

            return exam.FirstOrDefault()?.ExamID;

        }

        public async Task<IEnumerable<ExamDTO?>> GetExams(int page)
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
                })
                .Skip(CalculatePagination(page))
                .Take(LimitPerPage)
                .ToListAsync();
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




            return mapper.Map<ExamDTO>(exam);
        }

        public async Task<bool> Delete(int id)
        {
            var exam = await _appDBContext.Exams.FindAsync(id);
            if (exam == null)
            {
                throw new ResourceNotFoundException("Exam not found to be deleted");
            }
            _appDBContext.Exams.Remove(exam);
            await _appDBContext.SaveChangesAsync();
            return true;
        }

        public async Task<int?> Update(int id, ExamDTO entity)
        {
            var exam = await GetExamById(id);

            if (exam == null)
            {
                throw new ResourceNotFoundException("Exam not found to be updated");
            }

            exam.ExamDate = entity.ExamDate;

            await _appDBContext.SaveChangesAsync();

            return id;


        }

        public async Task<GetStudentExamResultsResult?> GetExamResultByStudentId(int studentId, int examId)
        {
            //TODO: Add Student Check 

            var exam = await GetExamById(examId);
            if (exam == null)
            {
                throw new ResourceNotFoundException("Exam not found");
            }
            var result = await appDBContextProcedures.GetStudentExamResultsAsync(studentId, examId);


            return result.FirstOrDefault();
        }

        public async Task<List<QuestionWithChoicesDTO>> GetExam(int id)
        {

            var isExamExist = await GetExamById(id);

            if (isExamExist == null)
            {
                throw new ResourceNotFoundException("No Exam with this ID");
            }

            var questions = await appDBContextProcedures.GetExamQuestionListAsync(id);
            if (questions == null || questions.Count == 0)
            {
                throw new ResourceNotFoundException("Exam has No Questions");
            }
            var choices = await appDBContextProcedures.GetExamChoiceListAsync(id);
            if (choices == null || choices.Count == 0)
            {
                throw new ResourceNotFoundException("No Choices for this Exam");
            }
            var exam = questions.Select(q => new QuestionWithChoicesDTO
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

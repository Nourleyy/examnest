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
                    ExamDate = e.ExamDate
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
            var exam = await _appDBContext.Exams.FindAsync(id);

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

            _appDBContext.Entry(exam).State = EntityState.Modified;
            await _appDBContext.SaveChangesAsync();

            return exam;


        }
    }
}

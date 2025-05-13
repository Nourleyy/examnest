using AutoMapper;
using ExamNest.DTO;
using ExamNest.Errors;
using ExamNest.Interfaces;
using ExamNest.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ExamNest.Repositories
{

    public class SubmissionRepository : GenericRepository, ISubmissionRepository
    {
        private readonly IMapper mapper;
        public SubmissionRepository(AppDBContext appDB, IMapper _mapper) : base(appDB)
        {
            mapper = _mapper;
        }

        public async Task<decimal?> Create(SubmissionInputDTO submission)
        {
            var answersJson = JsonConvert.SerializeObject(submission.Answers);
            var result = await appDBContextProcedures.SubmitExamAnswersAsync(submission.ExamID, submission.StudentID, answersJson);
            return result.FirstOrDefault()?.SubmissionID;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await GetById(id);

            if (entity == null)
            {
                throw new ResourceNotFoundException("Submission Not Found To Be Deleted");
            }
            _appDBContext.ExamSubmissions.Remove(entity); // To be replaced with the soft delete method
            await _appDBContext.SaveChangesAsync();
            return true;

        }

        public async Task<List<GetStudentExamAnswerDetailsResult>> GetSubmissionDetails(int id)
        {
            var submission = await GetById(id);
            if (submission == null)
            {
                throw new ResourceNotFoundException("Submission Not Found");
            }
            var SubmssionDetails = await appDBContextProcedures.GetStudentExamAnswerDetailsAsync(id);

            return SubmssionDetails;
        }

        public async Task<ExamSubmission?> GetById(int id)
        {

            var submission = await _appDBContext.ExamSubmissions
                .FirstOrDefaultAsync(x => x.SubmissionId == id);

            return submission;

        }

        public async Task<int?> Update(int id, SubmissionInputDTO entity)
        {
            // Find the Submission by ID
            var submission = await GetById(id);
            if (submission == null)
            {
                throw new ResourceNotFoundException("Submission Not Found To Be Updated");
            }
            submission.StudentAnswers = mapper.Map<List<StudentAnswer>>(entity.Answers);
            _appDBContext.Entry(submission).State = EntityState.Modified;
            await _appDBContext.SaveChangesAsync();

            return id;

        }

        public async Task<IEnumerable<SubmissionDTO>> GetAll(int page)
        {
            var submissions = await _appDBContext.ExamSubmissions
               .Include(s => s.Student)
               .ThenInclude(st => st.User)
               .Include(s => s.Exam)
               .ThenInclude(st => st.Course)
               .Skip(CalculatePagination(page))
               .Take(LimitPerPage)
               .ToListAsync();

            return mapper.Map<List<SubmissionDTO>>(submissions);
        }

        public async Task<List<GetStudentExamChoiceDetailsResult>> GetStudentExamChoiceDetailsAsync(int id)
        {
            var submission = await GetById(id);
            if (submission == null)
            {
                throw new ResourceNotFoundException("Submission Not Found");
            }
            return await appDBContextProcedures.GetStudentExamChoiceDetailsAsync(id);
        }
    }
}

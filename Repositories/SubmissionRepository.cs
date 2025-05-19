using AutoMapper;
using ExamNest.DTO.Submission;
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
        private readonly IExamRepository examRepository;
        public SubmissionRepository(AppDBContext appDB, IExamRepository examRepository, IMapper _mapper) : base(appDB)
        {
            mapper = _mapper;
            this.examRepository = examRepository;
        }

        public async Task<decimal?> Create(SubmissionPayload submission)
        {
            var exam = await examRepository.GetExamById(submission.ExamID);
            if (exam == null)
            {
                throw new ResourceNotFoundException("Exam not found!");
            }
            if (exam.ExamDate < DateTime.UtcNow)
            {
                throw new InvalidOperationException("Cannot submit exam after the exam date ;)");
            }
            var answersJson = JsonConvert.SerializeObject(submission.Answers);
            var result = await AppDbContextProcedures.SubmitExamAnswersAsync(submission.ExamID, submission.StudentID, answersJson);

            return result.FirstOrDefault()?.SubmissionID;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await GetById(id);

            if (entity == null)
            {
                throw new ResourceNotFoundException("Submission Not Found To Be Deleted");
            }
            AppDbContext.ExamSubmissions.Remove(entity); // To be replaced with the soft delete method
            await AppDbContext.SaveChangesAsync();
            return true;

        }

        public async Task<List<GetStudentExamAnswerDetailsResult>> GetSubmissionDetails(int id)
        {
            var submission = await GetById(id);
            if (submission == null)
            {
                throw new ResourceNotFoundException("Submission Not Found");
            }
            var SubmssionDetails = await AppDbContextProcedures.GetStudentExamAnswerDetailsAsync(id);

            return SubmssionDetails;
        }

        public async Task<ExamSubmission?> GetById(int id)
        {

            var submission = await AppDbContext.ExamSubmissions
                                .Include(x => x.Exam)
                                .Include(x => x.Student)
                                .ThenInclude(y => y.User)
                                .FirstOrDefaultAsync(x => x.SubmissionId == id);

            return submission;

        }

        public async Task<int?> Update(int id, SubmissionPayload entity)
        {
            // Find the Submission by ID
            var submission = await GetById(id);

            if (submission == null)
            {
                throw new ResourceNotFoundException("Submission Not Found To Be Updated");
            }
            // Check if the exam date is in the past
            if (submission.Exam.ExamDate < DateTime.UtcNow)
            {
                throw new InvalidOperationException("Cannot update submission after the exam date ;)");
            }
            submission.StudentAnswers = mapper.Map<List<StudentAnswer>>(entity.Answers);
            AppDbContext.Entry(submission).State = EntityState.Modified;
            await AppDbContext.SaveChangesAsync();

            return id;

        }

        public async Task<IEnumerable<SubmissionView>> GetAll(int page)
        {
            var submissions = await AppDbContext.ExamSubmissions
               .Include(s => s.Student)
               .ThenInclude(st => st.User)
               .Include(s => s.Exam)
               .ThenInclude(st => st.Course)
               .Skip(CalculatePagination(page))
               .Take(LimitPerPage)
               .ToListAsync();

            return mapper.Map<List<SubmissionView>>(submissions);
        }

        public async Task<List<GetStudentExamChoiceDetailsResult>> GetStudentExamChoiceDetailsAsync(int id)
        {
            var submission = await GetById(id);
            if (submission == null)
            {
                throw new ResourceNotFoundException("Submission Not Found");
            }
            return await AppDbContextProcedures.GetStudentExamChoiceDetailsAsync(id);
        }
    }
}

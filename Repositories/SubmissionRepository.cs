using AutoMapper;
using ExamNest.DTO;
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

        public async Task<SubmissionInputDTO?> Create(SubmissionInputDTO submission)
        {
            var answersJson = JsonConvert.SerializeObject(submission.Answers);
            var result = await appDBContextProcedures.SubmitExamAnswersAsync(submission.ExamID, submission.StudentID, answersJson);
             return submission;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await GetById( id);

            if (entity == null)
            {
                return false;
            }
            _appDBContext.ExamSubmissions.Remove(entity); // To be replaced with the soft delete method
            _appDBContext.SaveChanges();
            return true;

        }

        public async Task<List<GetStudentExamAnswerDetailsResult>> GetSubmissionDetails(int id)
        {
            var SubmssionDetails =  await appDBContextProcedures.GetStudentExamAnswerDetailsAsync(id);
         
            return SubmssionDetails;
        }

        public async Task<ExamSubmission?> GetById(int id)
        {

            var submission = await _appDBContext.ExamSubmissions
                .FirstOrDefaultAsync(x => x.SubmissionId == id);
          
            return submission ;

        }

        public Task<SubmissionInputDTO?> Update(int id, SubmissionInputDTO entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SubmissionDTO>> GetAll()
        {
            var submissions = await _appDBContext.ExamSubmissions
               .Include(s => s.Student)
               .ThenInclude(st => st.User)
               .Include(s => s.Exam)
               .ThenInclude(st => st.Course)
               .ToListAsync();
          
                return mapper.Map<List<SubmissionDTO>>(submissions);
        }

        public Task<List<GetStudentExamChoiceDetailsResult>> GetStudentExamChoiceDetailsAsync(int id)
        {
            return appDBContextProcedures.GetStudentExamChoiceDetailsAsync(id);
        }
    }
}

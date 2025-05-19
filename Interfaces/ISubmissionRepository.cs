using ExamNest.DTO.Submission;
using ExamNest.Models;

namespace ExamNest.Interfaces
{
    public interface ISubmissionRepository : IGeneric<SubmissionPayload>
    {
        Task<IEnumerable<SubmissionView>> GetAll(int page);
        Task<ExamSubmission?> GetById(int id);
        Task<List<GetStudentExamChoiceDetailsResult>> GetStudentExamChoiceDetailsAsync(int id);
        Task<List<GetStudentExamAnswerDetailsResult>> GetSubmissionDetails(int id);
    }
}
using ExamNest.DTO;
using ExamNest.Models;

namespace ExamNest.Interfaces
{
    public interface ISubmissionRepository
    {
        Task<List<SubmissionDTO>> GetAll();
        Task<ExamSubmission?> GetById(int id);
        Task<List<GetStudentExamChoiceDetailsResult>> GetStudentExamChoiceDetailsAsync(int id);
        Task<List<GetStudentExamAnswerDetailsResult>> GetSubmissionDetails(int id);
    }
}
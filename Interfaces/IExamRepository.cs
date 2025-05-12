using ExamNest.DTO;
using ExamNest.Models;

namespace ExamNest.Interfaces;

public interface IExamRepository : IGeneric<ExamDTO>
{
    public Task<GetExamDetailsResult?> GetExamDetailsById(int id);
    public Task<IEnumerable<ExamDTO?>> GetExams(int page);
    public Task<ExamDTO?> GetExamById(int id);

    public Task<GetStudentExamResultsResult?> GetExamResultByStudentId(int studentId, int examId);

    public Task<List<QuestionWithChoicesDTO>> GetExam(int id);


}
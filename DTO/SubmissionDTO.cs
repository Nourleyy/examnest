using FluentValidation;

namespace ExamNest.DTO
{
    public class SubmissionDTO
    {
        public int ExamId { get; set; }
        public int StudentId { get; set; }
        public string Name { get; set; }
        public string CourseName { get; set; }
        public DateTime SubmissionDate { get; set; }
        public decimal? Score { get; set; }
    }
    
}

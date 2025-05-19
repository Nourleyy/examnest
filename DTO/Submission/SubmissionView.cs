namespace ExamNest.DTO.Submission
{
    public class SubmissionView
    {
        public int ExamId { get; set; }
        public int StudentId { get; set; }
        public string Name { get; set; }
        public string CourseName { get; set; }
        public DateTime SubmissionDate { get; set; }
        public decimal? Score { get; set; }
    }

}

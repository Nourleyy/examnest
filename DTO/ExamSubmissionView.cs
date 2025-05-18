namespace ExamNest.DTO
{
    public class ExamSubmissionView
    {
        public int SubmissionId { get; set; }

        public DateTime SubmissionDate { get; set; }
        public decimal? Score { get; set; }

        public string StudentName { get; set; }

    }
}

namespace ExamNest.Models
{
    public class ExamQuestion
    {
        public int ExamId { get; set; }
        public int QuestionId { get; set; }

        public virtual Exam Exam { get; set; }
        public virtual QuestionBank Question { get; set; }
    }
}

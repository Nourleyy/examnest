using FluentValidation;

namespace ExamNest.DTO
{
    public class SubmissionInputDTO
    {
        public int ExamID { get; set; }
        public int StudentID { get; set; }
        public List<ExamAnswerDTO>? Answers { get; set; }
    }
    public class SubmissionDTOValidator : AbstractValidator<SubmissionDTO>
    {
        public SubmissionDTOValidator()
        {
            RuleFor(s => s.ExamId)
                  .NotEmpty().WithMessage("Exam Id is required.")
                  .GreaterThan(0).WithMessage("Exam Id must be greater than 0.");

            RuleFor(s => s.StudentId)
                  .NotEmpty().WithMessage("Student Id is required.")
                  .GreaterThan(0).WithMessage("Student Id must be greater than 0.");

        }
    }
}

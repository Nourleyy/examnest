using FluentValidation;

namespace ExamNest.DTO
{
    public class ExamAnswerDTO
    {
        public int QuestionID { get; set; }
        public string? StudentAnswer { get; set; }
    }
    class ExamAnswerValidator : AbstractValidator<ExamAnswerDTO>
    {
        public ExamAnswerValidator()
        {
            RuleFor(x => x.QuestionID)
                .GreaterThan(0).WithMessage("Question ID must be greater than 0.")
                .NotEmpty().WithMessage("Question ID is required.");
            RuleFor(x => x.StudentAnswer).NotEmpty().WithMessage("Student answer is required.");
        }
    }
}

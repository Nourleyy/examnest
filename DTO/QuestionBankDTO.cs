using FluentValidation;

namespace ExamNest.DTO
{
    public class QuestionBankDTO
    {
        public int CourseId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        public string ModelAnswer { get; set; }
        public int Points { get; set; }
    }
    public class QuestionBankDTOValidator : AbstractValidator<QuestionBankDTO>
    {
        public QuestionBankDTOValidator()
        {
            RuleFor(c => c.CourseId)
                  .NotEmpty().WithMessage("Course Id is required.")
                  .GreaterThan(0).WithMessage("Course Id must be greater than 0.");

            RuleFor(t => t.QuestionText)
                   .NotEmpty().WithMessage("Question Text is required.")
                   .MaximumLength(100).WithMessage("QuestionText must not exceed 100 characters.");

            RuleFor(t => t.QuestionType)
                   .NotEmpty().WithMessage("Question Type is required.")
                   .MaximumLength(3).WithMessage("Question Type must not exceed 3 characters.");

            RuleFor(t => t.ModelAnswer)
                   .NotEmpty().WithMessage("Model Answer is required.")
                   .MaximumLength(1).WithMessage("Model Answer must not exceed 1 character.");

            RuleFor(p => p.Points)
                 .NotEmpty().WithMessage("Points for each question is required.")
                 .GreaterThan(0).WithMessage("Question Point must be greater than 0.");

        }
    }

}

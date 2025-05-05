using FluentValidation;

namespace ExamNest.DTO
{
    public class ExamInputDTO
    {
        public int CourseId { get; set; }

        public int NoOfQuestions { get; set; }
        public DateTime ExamDate { get; set; }
    }

    public class ExamInputDTOValidator : AbstractValidator<ExamInputDTO>
    {
        public ExamInputDTOValidator()
        {
            RuleFor(n => n.NoOfQuestions)
                .NotEmpty()
                .GreaterThan(0).WithMessage("Number of Questions must be grater than zero.");

            RuleFor(e => e.CourseId)
                .NotEmpty()
                .GreaterThan(0).WithMessage("Course Id must be greater than zero.");

            RuleFor(e => e.ExamDate)
            .NotEmpty().WithMessage("Exam Date is required.")
            .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Exam Date cannot be in the past.");

        }
    }
}
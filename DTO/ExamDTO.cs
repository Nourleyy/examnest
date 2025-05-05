using FluentValidation;

namespace ExamNest.DTO
{
    public class ExamDTO
    {
        public int ExamId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public DateTime ExamDate { get; set; }
    }

    public class ExamDTOValidator : AbstractValidator<ExamDTO>
    {
        public ExamDTOValidator()
        {
            RuleFor(e => e.ExamId)
                .NotEmpty()
                .GreaterThan(0).WithMessage("Exam Id must be grater than zero.");

            RuleFor(e => e.CourseId)
                .NotEmpty()
                .GreaterThan(0).WithMessage("Course Id must be greater than zero.");

            RuleFor(e => e.ExamDate)
            .NotEmpty().WithMessage("Exam Date is required.")
            .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Exam Date cannot be in the past.");
        }
    }
}

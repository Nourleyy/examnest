using FluentValidation;

namespace ExamNest.DTO.Exam
{
    public class ExamCreatePayload
    {
        public int CourseId { get; set; }
        public int NoOfQuestions { get; set; }
        public DateTime ExamDate { get; set; }
        public DateTime EndDate { get; set; } = DateTime.Now.AddHours(1);
    }

    public class ExamCreatePayloadValidator : AbstractValidator<ExamCreatePayload>
    {
        public ExamCreatePayloadValidator()
        {
            RuleFor(n => n.NoOfQuestions)
                .NotEmpty()
                .GreaterThan(0).WithMessage("Number of Questions must be grater than zero.");

            RuleFor(e => e.CourseId)
                .NotEmpty()
                .GreaterThan(0).WithMessage("Course Id must be greater than zero.");

            RuleFor(e => e.ExamDate)
            .NotEmpty().WithMessage("Exam Date is required.")
            .GreaterThanOrEqualTo(_ => DateTime.Now).WithMessage("Exam Date cannot be in the past.");

            RuleFor(e => e.EndDate)
                .NotEmpty().WithMessage("End Date is required.")
                .GreaterThan(e => e.ExamDate).WithMessage("End Date must be greater than or equal to Exam Date.")
                .GreaterThanOrEqualTo(_ => DateTime.Now).WithMessage("End Date cannot be in the past.");

        }
    }
}
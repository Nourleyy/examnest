using FluentValidation;

namespace ExamNest.DTO.Exam
{
    public class ExamUpdatePayloadDTO
    {
        public int Id { get; set; }
        public DateTime ExamDate { get; set; }

        public DateTime EndDate { get; set; }
    }

    public class ExamUpdateValidator : AbstractValidator<ExamUpdatePayloadDTO>
    {
        public ExamUpdateValidator()
        {

            RuleFor(x => x.Id)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("Exam Id must be greater than zero.");

            RuleFor(x => x.ExamDate)
                .NotEmpty().WithMessage("Exam Date is required.")
                .GreaterThanOrEqualTo(_ => DateTime.Now)
                .WithMessage("Exam Date cannot be in the past. -_-");
            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End Date is required.")
                .GreaterThanOrEqualTo(x => x.ExamDate)
                .WithMessage("End Date must be greater than or equal to Exam Date.");
        }
    }
}

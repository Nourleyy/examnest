using FluentValidation;

namespace ExamNest.DTO
{
    public class ExamUpdatePayloadDTO
    {

        public int Id { get; set; }
        public DateTime ExamDate { get; set; }

    }

    class ExamUpdatePayloadValidation : AbstractValidator<ExamUpdatePayloadDTO>
    {
        public ExamUpdatePayloadValidation()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .GreaterThan(0).WithMessage("Exam Id must be greater than zero.");
            RuleFor(x => x.ExamDate)
                .NotEmpty().WithMessage("Exam Date is required.")
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Exam Date cannot be in the past. -_-");
        }
    }
}

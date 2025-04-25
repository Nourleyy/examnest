using FluentValidation;

namespace ExamNest.DTO
{
    public class TrackDTO
    {
        public int BranchId { get; set; }
        public string TrackName { get; set; }
    }
    public class TrackDTOValidator : AbstractValidator<TrackDTO>
    {
        public TrackDTOValidator()
        {
            RuleFor(t => t.TrackName)
            .NotEmpty().WithMessage("Track name is required.")
            .MaximumLength(50).WithMessage("Track name must not exceed 50 characters.");

            RuleFor(t => t.BranchId)
                .NotEmpty().WithMessage("Branch Id is required.")
                .GreaterThan(0).WithMessage("Branch Id must be greater than 0.");
        }
    }
}

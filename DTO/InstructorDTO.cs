using FluentValidation;

namespace ExamNest.DTO
{
    public class InstructorDTO
    {
        public int BranchId { get; set; }
        public int TrackId { get; set; }
        public int UserId { get; set; }
    }
    public class InstructorDTOValidator : AbstractValidator<InstructorDTO>
    {
        public InstructorDTOValidator()
        {
            RuleFor(b => b.BranchId)
                 .NotEmpty().WithMessage("Branch Id is required.")
                 .GreaterThan(0).WithMessage("Branch Id must be greater than 0.");

            RuleFor(t => t.TrackId)
                  .NotEmpty().WithMessage("Track Id is required.")
                  .GreaterThan(0).WithMessage("Track Id must be greater than 0.");


            RuleFor(u => u.UserId)
                  .NotEmpty().WithMessage("User Id is required.")
                  .GreaterThan(0).WithMessage("User Id must be greater than 0.");
        }
    }
}

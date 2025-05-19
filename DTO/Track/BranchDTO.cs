using FluentValidation;

namespace ExamNest.DTO.Track
{
    public class BranchDTO
    {
        public string BranchName { get; set; }
    }
    public class BranchDTOValidator : AbstractValidator<BranchDTO>
    {
        public BranchDTOValidator()
        {
            RuleFor(b => b.BranchName)
            .NotEmpty().WithMessage("Branch name is required.")
            .MaximumLength(50).WithMessage("Branch name must not exceed 50 characters.");
        }
    }
}

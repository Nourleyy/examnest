using FluentValidation;

namespace ExamNest.DTO.Student
{
    public class UpdateDto
    {
        public int BranchId { get; set; }
        public int TrackId { get; set; }
        public string UserId { get; set; }
    }

    public class StudentUpdateDto : UpdateDto
    {
    }
    public class InstructorUpdateDto : UpdateDto
    {
    }

    public class UpdateDtoValidator : AbstractValidator<UpdateDto>
    {
        public UpdateDtoValidator()
        {
            RuleFor(b => b.BranchId)
                 .NotEmpty().WithMessage("Branch Id is required.")
                 .GreaterThan(0).WithMessage("Branch Id must be greater than 0.");

            RuleFor(t => t.TrackId)
                  .NotEmpty().WithMessage("Track Id is required.")
                  .GreaterThan(0).WithMessage("Track Id must be greater than 0.");


            RuleFor(u => u.UserId)
                .NotEmpty().WithMessage("User Id is required.")
                .Length(36).WithMessage("User Id length is not correct");
        }
    }
}

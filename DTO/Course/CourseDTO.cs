using FluentValidation;

namespace ExamNest.DTO.Course
{
    public class CourseDTO
    {
        public int TrackId { get; set; }

        public string CourseName { get; set; }
    }
    public class CourseDTOValidator : AbstractValidator<CourseDTO>
    {
        public CourseDTOValidator()
        {
            RuleFor(c => c.CourseName)
                .NotEmpty().WithMessage("Course name is required.")
                .MaximumLength(50).WithMessage("Course name must not exceed 50 characters.");

            RuleFor(t => t.TrackId)
                  .NotEmpty().WithMessage("Track Id is required.")
                  .GreaterThan(0).WithMessage("Track Id must be greater than 0.");
        }
    }

}

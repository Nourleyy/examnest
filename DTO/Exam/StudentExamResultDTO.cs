using FluentValidation;

namespace ExamNest.DTO.Exam
{
    public class StudentExamResultDTO
    {
        public int StudentId { get; set; }
        public int? ExamId { get; set; }
    }

    public class StudentExamResultDTOValidator : AbstractValidator<StudentExamResultDTO>
    {
        public StudentExamResultDTOValidator()
        {
            RuleFor(s => s.ExamId)
                .GreaterThan(0).WithMessage("Exam Id must be grater than zero.");

            RuleFor(e => e.StudentId)
                .NotEmpty()
                .GreaterThan(0).WithMessage("Student Id must be greater than zero.");
        }
    }
}

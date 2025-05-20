using ExamNest.DTO.Exam;
using FluentValidation;

namespace ExamNest.DTO.Submission
{
    public class SubmissionPayload
    {
        public int ExamID { get; set; }
        public List<ExamAnswerDTO>? Answers { get; set; }
    }
    public class SubmissionInputDTOValidator : AbstractValidator<SubmissionPayload>
    {
        public SubmissionInputDTOValidator()
        {
            RuleFor(s => s.ExamID)
                  .NotEmpty().WithMessage("Exam ID is required.")
                  .GreaterThan(0).WithMessage("Exam ID must be greater than 0.");


        }
    }

    public class SubmssionDTO : SubmissionPayload
    {
        public string UserId { get; set; }
    }


}

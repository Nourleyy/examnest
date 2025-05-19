using FluentValidation;

namespace ExamNest.DTO.Question
{
    public class ChoiceDTO
    {
        public int QuestionId { get; set; }
        public string ChoiceLetter { get; set; }
        public string ChoiceText { get; set; }
    }
    public class ChoiceDTOValidator : AbstractValidator<ChoiceDTO>
    {
        public ChoiceDTOValidator()
        {
            RuleFor(q => q.QuestionId)
                  .NotEmpty().WithMessage("Question Id is required.")
                  .GreaterThan(0).WithMessage("Question Id must be greater than 0.");

            RuleFor(t => t.ChoiceLetter)
                   .NotEmpty().WithMessage("ChoiceLetter is required.")
                   .MaximumLength(1).WithMessage("ChoiceLetter must not exceed 1 character.");

            RuleFor(t => t.ChoiceText)
                   .NotEmpty().WithMessage("ChoiceText is required.")
                   .MaximumLength(60).WithMessage("ChoiceText must not exceed 60 characters.");

        }
    }
}

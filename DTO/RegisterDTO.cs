using FluentValidation;

namespace ExamNest.DTO
{
    public class RegisterDTO : AuthenticationDTO
    {
        public string Name { get; set; }

    }
    public class RegisterDTOValidation : AbstractValidator<RegisterDTO>
    {
        public RegisterDTOValidation()
        {
            Include(new AuthenticationDTOValidation());

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");
        }
    }
}

using FluentValidation;

namespace ExamNest.DTO.Authentication
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
                .Matches(@"^[A-Za-z\s]*$").WithMessage("{PropertyName} should only contain letters.")

                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}

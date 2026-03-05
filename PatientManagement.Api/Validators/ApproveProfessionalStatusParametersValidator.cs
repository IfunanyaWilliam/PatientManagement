
namespace PatientManagement.Api.Validators
{
    using FluentValidation;
    using Parameters;


    public class ApproveProfessionalStatusParametersValidator : AbstractValidator<ApproveProfessionalStatusParameters>
    {
        public ApproveProfessionalStatusParametersValidator()
        {
            RuleFor(x => x.ProfessionalId)
                .NotEmpty().WithMessage("ProfessionalId is required")
                .NotNull().WithMessage("ProfessionalId cannot be null");
        }
    }
}


namespace PatientManagement.Api.Validators
{
    using FluentValidation;
    using PatientManagement.Api.Parameters;


    public class UpdatePatientParametersValidator : AbstractValidator<UpdatePatientParameters>
    {
        public UpdatePatientParametersValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required")
                .NotNull().WithMessage("Id cannot be null");

            RuleFor(x => x.ApplicationUserId)
                .NotEmpty().WithMessage("ApplicationUserId is required")
                .NotNull().WithMessage("ApplicationUserId cannot be null");

            RuleFor(x => x.Title)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Title is required")
                .NotNull().WithMessage("Title cannot be null")
                .MinimumLength(2).WithMessage("Title must be at least 2 characters long")
                .MaximumLength(50).WithMessage("Title cannot exceed 50 characters")
                .Matches(@"^[a-zA-Z]+$").WithMessage("Title must contain only letters");

            RuleFor(x => x.FirstName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("First Name is required")
                .NotNull().WithMessage("First Name cannot be null")
                .MinimumLength(2).WithMessage("First Name must be at least 2 characters long")
                .MaximumLength(100).WithMessage("First Name cannot exceed 100 characters")
                .Matches(@"^[a-zA-Z]+$").WithMessage("First Name must contain only letters");

            RuleFor(x => x.MiddleName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Middle Name is required")
                .NotNull().WithMessage("Middle Name cannot be null")
                .MinimumLength(2).WithMessage("Middle Name must be at least 2 characters long")
                .MaximumLength(100).WithMessage("Middle Name cannot exceed 100 characters")
                .Matches(@"^[a-zA-Z]+$").WithMessage("Middle Name must contain only letters");

            RuleFor(x => x.LastName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Last Name is required")
                .NotNull().WithMessage("Last Name cannot be null")
                .MinimumLength(2).WithMessage("Last Name must be at least 2 characters long")
                .MaximumLength(100).WithMessage("Last Name cannot exceed 100 characters")
                .Matches(@"^[a-zA-Z]+$").WithMessage("Last Name must contain only letters");

            RuleFor(x => x.PhoneNumber)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Phone Number is required")
                .NotNull().WithMessage("Phone Number cannot be null")
                .Matches(@"^(\+234|0)[7-9][0-1]\d{8}$").WithMessage("Phone Number must be a valid Nigerian phone number");

            RuleFor(x => x.Age)
                .InclusiveBetween(18, 65).WithMessage("Age must be between 18 and 100");
        }
    }
}

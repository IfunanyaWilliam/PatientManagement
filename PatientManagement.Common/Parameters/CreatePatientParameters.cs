
namespace PatientManagement.Common.Parameters
{
    public class CreatePatientParameters
    {
        public CreatePatientParameters()
        {
        }

        public CreatePatientParameters(
            Guid applicationUserId,
            string? title,
            string? firstName,
            string? middleName,
            string? lastName,
            string? phoneNumber,
            int age)
        {
            ApplicationUserId = applicationUserId;
            Title = title;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Age = age;
        }

        public Guid ApplicationUserId { get; set; }
        public string? Title { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public int Age { get; set; }
    }
}


namespace PatientManagement.Api.Parameters
{
    using System.ComponentModel.DataAnnotations;

    public class GetAuthTokenParameter
    {
        public GetAuthTokenParameter(
            string email,
            string password)
        {
            Email = email;
            Password = password;
        }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Required]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
    }
}

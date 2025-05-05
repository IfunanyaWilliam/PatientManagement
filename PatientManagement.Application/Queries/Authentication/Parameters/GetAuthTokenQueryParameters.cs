
namespace PatientManagement.Application.Queries.Authentication.Parameters
{
    using Interfaces.Queries;


    public class GetAuthTokenQueryParameters : IQueryParameters
    {
        public GetAuthTokenQueryParameters(
            string email,
            string password)
        {
            Email = email;
            Password = password;
        }
        
        public string Email { get; set; }

        public string Password { get; set; }
    }
}

namespace PatientManagement.Api.Parameters
{
    public class LoginWithFacebookParameters
    {
        public LoginWithFacebookParameters()
        {
        }

        public LoginWithFacebookParameters(string accessToken)
        {
            AccessToken = accessToken;
        }

        public string AccessToken { get; set; }
    }
}

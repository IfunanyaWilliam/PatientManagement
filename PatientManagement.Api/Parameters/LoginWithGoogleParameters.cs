namespace PatientManagement.Api.Parameters
{
    public class LoginWithGoogleParameters
    {
        public LoginWithGoogleParameters(string idToken)
        {
            IdToken = idToken;
        }

        public string IdToken { get; set; }
    }
}

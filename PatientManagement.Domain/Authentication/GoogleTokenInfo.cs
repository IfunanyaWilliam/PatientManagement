
namespace PatientManagement.Domain.Authentication
{
    public class GoogleTokenInfo
    {
        public string Sub { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool EmailVerified { get; set; }
        public string? GivenName { get; set; }
        public string? FamilyName { get; set; }
        public string? Picture { get; set; }
        public string Aud { get; set; } = string.Empty;
    }
}

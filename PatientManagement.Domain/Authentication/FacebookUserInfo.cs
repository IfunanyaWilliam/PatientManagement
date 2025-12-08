
namespace PatientManagement.Domain.Authentication
{
    public class FacebookUserInfo
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public FacebookPicture? Picture { get; set; }
    }
}

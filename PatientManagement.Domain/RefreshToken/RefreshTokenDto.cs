
namespace PatientManagement.Domain.RefreshToken
{
    public class RefreshTokenDto
    {
        public Guid ApplicationUserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}

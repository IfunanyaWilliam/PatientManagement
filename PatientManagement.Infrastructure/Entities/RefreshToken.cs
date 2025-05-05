
namespace PatientManagement.Infrastructure.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public Guid ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string Token { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow.AddHours(1);
        public DateTime? DateModified { get; set; } = DateTime.UtcNow.AddHours(1);
    }
}

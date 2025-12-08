
namespace PatientManagement.Infrastructure.Entities
{
    public class ExternalLogin
    {
        public Guid Id { get; set; }
        public Guid ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public string Provider { get; set; } = string.Empty; 
        public string ProviderUserId { get; set; } = string.Empty;
        public DateTimeOffset LinkedAt { get; set; }
    }
}

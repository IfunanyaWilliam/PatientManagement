
namespace PatientManagement.Infrastructure.Entities
{
    public class Permission
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string EncryptedName { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public DateTime DateModified { get; set; }
    }
}

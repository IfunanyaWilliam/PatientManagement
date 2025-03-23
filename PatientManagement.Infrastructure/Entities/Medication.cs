
namespace PatientManagement.Infrastructure.Entities
{
    public class Medication
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow.AddHours(1);
    }
}

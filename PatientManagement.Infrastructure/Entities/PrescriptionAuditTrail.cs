
namespace PatientManagement.Infrastructure.Entities
{
    public class PrescriptionAuditTrail
    {
        public Guid Id { get; set; }
        public Guid PrescriptionId { get; set; }
        public Guid ProfessionalId { get; set; }
        public Guid PatientId { get; set; }
        public Guid ConsultationId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow.AddHours(1);
    }
}

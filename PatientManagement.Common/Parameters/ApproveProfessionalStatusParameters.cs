
namespace PatientManagement.Common.Parameters
{
    using System.ComponentModel.DataAnnotations;
    public class ApproveProfessionalStatusParameters
    {
        public ApproveProfessionalStatusParameters(Guid professionalId)
        {
            ProfessionalId = professionalId;
        }

        [Required(ErrorMessage = "ProfessionalId is required")]
        public Guid ProfessionalId { get; set; }
    }
}

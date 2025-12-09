
namespace PatientManagement.Application.Interfaces.Services
{
    using PatientManagement.Domain.Authentication;


    public interface IGoogleAuthService
    {
        Task<GoogleUserInfo?> ValidateGoogleTokenAsync(string idToken);
    }
}

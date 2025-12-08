
namespace PatientManagement.Application.Interfaces.Services
{
    using Domain.Authentication;

    public interface IFacebookAuthService
    {
        Task<FacebookUserInfo?> VerifyFacebookTokenAsync(string accessToken);
    }
}

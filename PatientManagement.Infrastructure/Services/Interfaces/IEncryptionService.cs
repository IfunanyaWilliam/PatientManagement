
namespace PatientManagement.Infrastructure.Services.Interfaces
{
    public interface IEncryptionService
    {
        string Encrypt(string plainText);
        string Decrypt(string encryptedText);
    }
}

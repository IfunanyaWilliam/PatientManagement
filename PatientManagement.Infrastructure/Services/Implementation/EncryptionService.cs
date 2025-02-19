
namespace PatientManagement.Infrastructure.Services.Implementation
{
    using Microsoft.Extensions.Configuration;
    using System.Security.Cryptography;
    using Interfaces;

    public class EncryptionService : IEncryptionService
    {
        private readonly string _encryptionKey;
        private readonly string _initializationVector;

        public EncryptionService(IConfiguration configuration)
        {
            _encryptionKey = configuration["Encryption:Key"]
                ?? throw new ArgumentNullException("Encryption:Key must be configured");
            _initializationVector = configuration["Encryption:IV"]
                ?? throw new ArgumentNullException("Encryption:IV must be configured");
        }

        public string Encrypt(string plainText)
        {
            using var aesAlgorithm = Aes.Create();
            aesAlgorithm.Key = Convert.FromBase64String(_encryptionKey);
            aesAlgorithm.IV = Convert.FromBase64String(_initializationVector);

            var encryptionTransform = aesAlgorithm.CreateEncryptor(aesAlgorithm.Key, aesAlgorithm.IV);

            using var encryptionStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(encryptionStream, encryptionTransform, CryptoStreamMode.Write);
            using (var encryptionWriter = new StreamWriter(cryptoStream))
            {
                encryptionWriter.Write(plainText);
            }

            var encryptedBytes = encryptionStream.ToArray();
            return Convert.ToBase64String(encryptedBytes);
        }

        public string Decrypt(string encryptedText)
        {
            using var aesAlgorithm = Aes.Create();
            aesAlgorithm.Key = Convert.FromBase64String(_encryptionKey);
            aesAlgorithm.IV = Convert.FromBase64String(_initializationVector);

            var decryptionTransform = aesAlgorithm.CreateDecryptor(aesAlgorithm.Key, aesAlgorithm.IV);

            var encryptedBytes = Convert.FromBase64String(encryptedText);
            using var decryptionStream = new MemoryStream(encryptedBytes);
            using var cryptoStream = new CryptoStream(decryptionStream, decryptionTransform, CryptoStreamMode.Read);
            using var decryptionReader = new StreamReader(cryptoStream);

            return decryptionReader.ReadToEnd();
        }
    }
}

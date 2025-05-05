
namespace PatientManagement.Domain.Permission
{
    public class Permission
    {
        public Permission(
            Guid id,
            string encryptedName,
            DateTime dateCreated,
            DateTime dateModified)
        {
            Id = id;
            EncryptedName = encryptedName;
            DateCreated = dateCreated;
            DateModified = dateModified;
        }

        public Guid Id { get; set; } = Guid.NewGuid();
        public string EncryptedName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}

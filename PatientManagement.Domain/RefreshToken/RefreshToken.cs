
namespace PatientManagement.Domain.RefreshToken
{
    using ApplicationUser;
    public class RefreshToken
    {
        public RefreshToken(
            Guid id,
            ApplicationUser user,
            string token,
            bool isRevoked,
            DateTime expiresAt,
            DateTime dateCreated,
            DateTime? dateModified)
        {
            Id = id;
            ApplicationUser = user;
            Token = token;
            IsRevoked = isRevoked;
            ExpiresAt = expiresAt;
            DateCreated = dateCreated;
            DateModified = dateModified;
        }


        public Guid Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string Token { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}

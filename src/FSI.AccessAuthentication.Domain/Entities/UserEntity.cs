namespace FSI.AccessAuthentication.Domain.Entities
{
    public class UserEntity : BaseEntity
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public long SystemId { get; set; }
        public long ProfileId { get; set; }
     }
}

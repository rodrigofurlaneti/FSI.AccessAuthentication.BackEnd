namespace FSI.AccessAuthentication.Domain.Entities
{
    public class AuthenticationEntity : BaseEntity
    {
        public int SystemId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsAuthentication { get; set; }
        public DateTime Expiration { get; set; }
    }
}

namespace Bookong.Application.DTOs
{
    public class LoginUserResponse
    {
        public Guid UserId { get; set; }
        public required string Message { get; set; }
        public bool IsFirstTimeUser { get; set; }
        public string? Username { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? OrganizationalUnit { get; set; }
    }
}

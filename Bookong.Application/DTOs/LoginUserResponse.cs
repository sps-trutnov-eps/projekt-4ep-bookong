namespace Bookong.Application.DTOs
{
    public class LoginUserResponse
    {
        public Guid UserId { get; set; }
        public required string Message { get; set; }
    }
}

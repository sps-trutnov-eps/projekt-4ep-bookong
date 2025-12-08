namespace Bookong.Application.DTOs
{
    public class CompleteUserRegistrationResponse
    {
        public Guid UserId { get; set; }
        public required string Message { get; set; }
        public bool Success { get; set; }
    }
}

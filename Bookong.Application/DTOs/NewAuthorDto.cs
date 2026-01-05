namespace Bookong.Application.DTOs
{
    public class NewAuthorDto
    {
        // Temporary negative id used on the client to reference this new item before persistence.
        public int TempId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
    }
}

namespace Bookong.Application.DTOs
{
    public class BookListItemDto
    {
        public Guid PublicId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string AuthorFullName { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Kind { get; set; } = string.Empty;
        public bool Borrowable { get; set; }
        public int LoanCount { get; set; }
    }
}

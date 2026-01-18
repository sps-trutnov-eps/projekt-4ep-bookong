namespace Bookong.Application.DTOs
{
    public class BookLoanStatusDto
    {
        public int BookId { get; set; }
        public Guid BookPublicId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public string BookAuthor { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public bool IsCurrentlyLoaned { get; set; }
        public int? CurrentLoanId { get; set; }
        public string? CurrentBorrowerName { get; set; }
        public DateTime? LoanDate { get; set; }
        public int? DaysLoaned { get; set; }
    }
}

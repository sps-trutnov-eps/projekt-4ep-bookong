namespace Bookong.Application.DTOs
{
    public class BookLoanListItemDto
    {
        public int Id { get; set; }
        public Guid PublicId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public string BookAuthor { get; set; } = string.Empty;
        public string UserFullName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsReturned => ReturnDate.HasValue;
        public int DaysLoaned => IsReturned 
            ? (ReturnDate!.Value - LoanDate).Days 
            : (DateTime.UtcNow - LoanDate).Days;
    }
}

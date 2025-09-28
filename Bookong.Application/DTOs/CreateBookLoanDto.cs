namespace Bookong.Application.DTOs
{
    public class CreateBookLoanDto
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}

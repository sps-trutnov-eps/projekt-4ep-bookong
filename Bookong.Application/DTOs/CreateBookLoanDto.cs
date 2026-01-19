namespace Bookong.Application.DTOs
{
    public class CreateBookLoanDto
    {
        public Guid BookPublicId { get; set; }
        public Guid UserPublicId { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}

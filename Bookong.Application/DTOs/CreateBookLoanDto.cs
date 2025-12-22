namespace Bookong.Application.DTOs
{
    public class CreateBookLoanDto
    {
        public Guid BookPublicId { get; set; }
        public Guid PublicId { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}

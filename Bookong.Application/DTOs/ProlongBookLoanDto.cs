namespace Bookong.Application.DTOs
{
    public class ProlongBookLoanDto
    {
        public Guid BookPublicId { get; set; }
        public int DaysToProlong { get; set; } = 14;
    }
}

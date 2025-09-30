using System.ComponentModel.DataAnnotations;

namespace Bookong.Domain.Entities
{
    public class BookLoan
    {
        [Key]
        public int Id { get; set; }

        public Guid PublicId { get; set; } = Guid.NewGuid();

        public int UserId { get; set; }
        public required User User { get; set; }

        public int BookId { get; set; }
        public required Book Book { get; set; }

        public DateTime LoanDate { get; set; } = DateTime.UtcNow;

        public DateTime? ReturnDate { get; set; }
    }
}

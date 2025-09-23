using System.ComponentModel.DataAnnotations;

namespace Bookong.Domain.Entities
{
    public class BookLoan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required Guid PublicId { get; set; } = Guid.NewGuid();

        [Required]
        public required int UserId { get; set; }
        [Required]
        public required User User { get; set; }

        [Required]
        public required int BookId { get; set; }
        [Required]
        public required Book Book { get; set; }

        [Required]
        public required DateTime LoanDate { get; set; } = DateTime.UtcNow;

        public DateTime? ReturnDate { get; set; }
    }
}

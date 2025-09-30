using System.ComponentModel.DataAnnotations;

namespace Bookong.Domain.Entities
{
    public class BookReservation
    {
        [Key]
        public int Id { get; set; }
        public Guid PublicId { get; set; } = Guid.NewGuid();

        public DateTime ReservationDate { get; set; } = DateTime.UtcNow;

        public int BookId { get; set; }
        public required Book Book { get; set; }

        public int UserId { get; set; }
        public required User User { get; set; }
    }
}

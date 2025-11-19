using System.ComponentModel.DataAnnotations;

namespace Bookong.Domain.Entities
{
    public class MaturitaAvailableBook
    {
        [Key]
        public int Id { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; } = null!;

        public int? CreatedByUserId { get; set; }
        // optional: timestamp
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
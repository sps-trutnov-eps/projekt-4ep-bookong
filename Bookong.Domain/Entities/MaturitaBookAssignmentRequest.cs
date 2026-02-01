using System.ComponentModel.DataAnnotations;

namespace Bookong.Domain.Entities
{
    public class MaturitaBookAssignmentRequest
    {
        [Key]
        public int Id { get; set; }
        public Guid PublicId { get; set; } = Guid.NewGuid();

        public required string BookName { get; set; }

        public required string BookAuthor { get; set; }

        public int RequestingUserId { get; set; }
        public required User RequestingUser { get; set; }

        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ProcessedAt { get; set; }

        public int? ProcessingUserId { get; set; }
        public User? ProcessingUser { get; set; }

        public string? Note { get; set; }

        public bool IsApproved { get; set; }
    }
}

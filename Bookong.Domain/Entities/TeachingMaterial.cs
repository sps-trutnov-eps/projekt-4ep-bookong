using System.ComponentModel.DataAnnotations;

namespace Bookong.Domain.Entities
{
    public class TeachingMaterial
    {
        [Key]
        public int Id { get; set; }
        public Guid PublicId { get; set; } = Guid.NewGuid();

        public required string Title { get; set; }

        public string? Description { get; set; }

        public required string Url { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}

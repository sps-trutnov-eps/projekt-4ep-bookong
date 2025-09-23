using System.ComponentModel.DataAnnotations;

namespace Bookong.Domain.Entities
{
    public class Period
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required Guid PublicId { get; set; } = Guid.NewGuid();

        [Required]
        public required string Name { get; set; }
    }
}

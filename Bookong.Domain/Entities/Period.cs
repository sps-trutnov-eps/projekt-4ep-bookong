using System.ComponentModel.DataAnnotations;

namespace Bookong.Domain.Entities
{
    public class Period
    {
        [Key]
        public int Id { get; set; }

        public Guid PublicId { get; set; } = Guid.NewGuid();

        public required string Name { get; set; }
    }
}

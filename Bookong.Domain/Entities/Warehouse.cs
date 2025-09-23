using System.ComponentModel.DataAnnotations;

namespace Bookong.Domain.Entities
{
    public class Warehouse
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required Guid PublicId { get; set; } = Guid.NewGuid();

        [Required]
        public required string Name { get; set; }

        [Required]
        public required int AddressId { get; set; }

        [Required]
        public required Address Address { get; set; }
    }
}

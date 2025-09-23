using System.ComponentModel.DataAnnotations;

namespace Bookong.Domain.Entities
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required Guid PublicId { get; set; } = Guid.NewGuid();

        [Required]
        public required string Street { get; set; }

        [Required]
        public required string Number { get; set; }

        [Required]
        public required string City { get; set; }

        [Required]
        public required string ZipCode { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Bookong.Domain.Entities
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        public Guid PublicId { get; set; } = Guid.NewGuid();

        public required string Street { get; set; }

        public required string Number { get; set; }

        public required string City { get; set; }

        public required string ZipCode { get; set; }
    }
}

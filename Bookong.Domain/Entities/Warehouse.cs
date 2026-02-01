using System.ComponentModel.DataAnnotations;

namespace Bookong.Domain.Entities
{
    public class Warehouse
    {
        [Key]
        public int Id { get; set; }

        public Guid PublicId { get; set; } = Guid.NewGuid();

        public required string Name { get; set; }

        public int AddressId { get; set; }
        public required Address Address { get; set; }
    }
}

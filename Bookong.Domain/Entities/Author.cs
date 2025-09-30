using System.ComponentModel.DataAnnotations;

namespace Bookong.Domain.Entities
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        public required Guid PublicId { get; set; } = Guid.NewGuid();

        public required string FirstName { get; set; }

        public required string MiddleName { get; set; }

        public required string LastName { get; set; }

        public DateTime ?DateOfBirth { get; set; }
    }
}

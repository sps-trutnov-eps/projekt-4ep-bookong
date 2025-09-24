using System.ComponentModel.DataAnnotations;

namespace Bookong.Domain.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required Guid PublicId { get; set; } = Guid.NewGuid();

        [Required]
        public required string SamAccountName { get; set; }

        [Required]
        public required Guid ObjectGuid { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}

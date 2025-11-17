using System.ComponentModel.DataAnnotations;

namespace Bookong.Domain.Entities
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }
        public Guid PublicId { get; set; }

        public required string Name { get; set; }
    }
}

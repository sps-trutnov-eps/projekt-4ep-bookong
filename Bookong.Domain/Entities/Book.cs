using System.ComponentModel.DataAnnotations;

namespace Bookong.Domain.Entities
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required Guid PublicId { get; set; } = Guid.NewGuid();

        [Required]
        public required string Name { get; set; }

        public string? ISBN { get; set; }

        [Required]
        public required int AuthorId { get; set; }
        [Required]
        public required Author Author { get; set; }

        public DateTime? DateRelease { get; set; }

        [Required]
        public required int GenreId { get; set; }
        [Required]
        public required Genre Genre { get; set; }

        [Required]
        public required int KindId { get; set; }
        [Required]
        public required Kind Kind { get; set; }

        [Required]
        public ushort Pages { get; set; }

        [Required]
        public required int PeriodId { get; set; }
        [Required]
        public required Period Period { get; set; }

        [Required]
        public required int WarehouseId { get; set; }
        [Required]
        public required Warehouse Warehouse { get; set; }
    }
}

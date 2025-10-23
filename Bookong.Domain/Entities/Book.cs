using System.ComponentModel.DataAnnotations;

namespace Bookong.Domain.Entities
{
    public class Book
    {
        // Identification
        [Key]
        public int Id { get; set; }
        public Guid PublicId { get; set; } = Guid.NewGuid();
        
        public required string Name { get; set; }
        public string? ISBN { get; set; }


        public int? AuthorId { get; set; }
        public Author? Author { get; set; }

        public int? GenreId { get; set; }
        public Genre? Genre { get; set; }

        public int? KindId { get; set; }
        public Kind? Kind { get; set; }

        public int? PeriodId { get; set; }
        public Period? Period { get; set; }


        public ushort? Pages { get; set; }


        public int? PublisherId { get; set; }
        public Publisher? Publisher { get; set; }

        public DateTime? DateRelease { get; set; }


        // Placement
        public int? WarehouseId { get; set; }
        public Warehouse? Warehouse { get; set; }


        // System
        public bool Borrowable { get; set; } = false;
    }
}
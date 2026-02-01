using System.ComponentModel.DataAnnotations;

namespace Bookong.Domain.Entities
{
    public class MaturitaBook
    {
        // Identification
        [Key]
        public int Id { get; set; }
        public Guid PublicId { get; set; } = Guid.NewGuid();
        
        public required string Name { get; set; }

        public int AuthorId { get; set; }
        public Author? Author { get; set; }


        // Information
        public int GenreId { get; set; }
        public Genre? Genre { get; set; }

        public int KindId { get; set; }
        public Kind? Kind { get; set; }

        public int PeriodId { get; set; }
        public Period? Period { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Bookong.Application.DTOs
{
    public class CreateBookDto
    {
        [Required(ErrorMessage = "Zadejte název knihy.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Vyberte autora.")]
        public int? AuthorId { get; set; }

        public string? ISBN { get; set; }

        [Required(ErrorMessage = "Vyberte žánr.")]
        public int? GenreId { get; set; }

        [Required(ErrorMessage = "Vyberte literární druh.")]
        public int? KindId { get; set; }

        [Required(ErrorMessage = "Vyberte historické období.")]
        public int? PeriodId { get; set; }

        [Range(1, 10000, ErrorMessage = "Počet stran musí být mezi 1 a 10 000.")]
        public ushort? Pages { get; set; }

        public DateTime? DateRelease { get; set; }

        public int? WarehouseId { get; set; }
    }
}

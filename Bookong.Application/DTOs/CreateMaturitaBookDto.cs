using System.ComponentModel.DataAnnotations;

namespace Bookong.Application.DTOs
{
    public class CreateMaturitaBookDto
    {
        [Required(ErrorMessage = "Zadejte název díla.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Zadejte autora.")]
        public int? AuthorId { get; set; }

        [Required(ErrorMessage = "Vyberte žánr.")]
        public int? GenreId { get; set; }
                  
        [Required(ErrorMessage = "Vyberte literární druh.")]
        public int? KindId { get; set; }
                  
        [Required(ErrorMessage = "Vyberte literární období.")]
        public int? PeriodId { get; set; }
    }
}

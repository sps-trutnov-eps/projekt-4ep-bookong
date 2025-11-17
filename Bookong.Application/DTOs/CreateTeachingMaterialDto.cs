using System.ComponentModel.DataAnnotations;

namespace Bookong.Application.DTOs
{
    public class CreateTeachingMaterialDto
    {
        [Required(ErrorMessage = "Zadejte titulek výukového materiálu.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Zadejte URL výukového materiálu.")]
        public string? Url { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Bookong.Application.DTOs
{
    public class CreatePublisherBasicDto
    {
        [Required(ErrorMessage = "Zadejte název vydavatelství.")]
        public string? Name { get; set; }
    }
}

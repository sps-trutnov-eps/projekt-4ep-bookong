using System.ComponentModel.DataAnnotations;

namespace Bookong.Application.DTOs
{
    public class CreatePublisherExtendedDto
    {
        [Required(ErrorMessage = "Zadejte název vydavatelství.")]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? RegistrationNumber { get; set; }

        public string? TaxId { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Website { get; set; }
    }
}
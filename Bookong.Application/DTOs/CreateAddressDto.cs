using System.ComponentModel.DataAnnotations;

namespace Bookong.Application.DTOs
{
    public class CreateAddressDto
    {
        [Required(ErrorMessage = "Zadejte název ulice.")]
        public string Street { get; set; } = "";

        [Required(ErrorMessage = "Zadejte číslo popisné.")]
        public string Number { get; set; } = "";

        [Required(ErrorMessage = "Zadejte jméno města.")]
        public string City { get; set; } = "";

        [Required(ErrorMessage = "Zadejte poštovní směrovací číslo.")]
        public string ZipCode { get; set; } = "";
    }
}

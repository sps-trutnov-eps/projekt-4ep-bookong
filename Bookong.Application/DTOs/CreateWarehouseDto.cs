using System.ComponentModel.DataAnnotations;

namespace Bookong.Application.DTOs
{
    public class CreateWarehouseDto
    {
        [Required(ErrorMessage = "Zadejte název skladu.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Vyplňte adresu skladu.")]
        public CreateAddressDto Address { get; set; } = new CreateAddressDto();
    }
}

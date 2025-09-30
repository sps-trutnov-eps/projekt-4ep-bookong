using System.ComponentModel.DataAnnotations;

namespace Bookong.Application.DTOs
{
    public class UpdateWarehouseDto
    {
        [Required]
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public UpdateAddressDto? Address { get; set; }
    }
}

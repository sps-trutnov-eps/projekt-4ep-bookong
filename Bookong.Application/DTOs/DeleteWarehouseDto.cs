using System.ComponentModel.DataAnnotations;

namespace Bookong.Application.DTOs
{
    public class DeleteWarehouseDto
    {
        [Required]
        public Guid WarehouseIdToDelete { get; set; }
    
        public Guid? TargetWarehouse { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Bookong.Application.DTOs
{
    public class CreateMaturitaBookAssignmentRequestDto
    {
        [Key]
        public int Id { get; set; }
        
        public Guid PublicId { get; set; }
        
        [Required(ErrorMessage = "Zadejte název díla.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Zadejte jméno autora.")]
        public string? Author { get; set; }

        public string? Note { get; set; }

        public int RequestingUserId { get; set; }
    }
}

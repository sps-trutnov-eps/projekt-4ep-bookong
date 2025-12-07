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

        // Use int? here so Blazor's InputNumber binds to a supported type. Will be cast to ushort when creating domain entity.
        [Range(1, 10000, ErrorMessage = "Počet stran musí být mezi 1 a 10 000.")]
        public int? Pages { get; set; }

        public DateTime? DateRelease { get; set; }

        public int? WarehouseId { get; set; }

        // Collections for new related entities created inline in the form.
        // Each new item carries a TempId (negative) which matches the temporary select value used in the UI.
        public List<NewAuthorDto>? NewAuthors { get; set; }
        public List<NewNameDto>? NewGenres { get; set; }
        public List<NewNameDto>? NewKinds { get; set; }
        public List<NewNameDto>? NewPeriods { get; set; }
        public List<NewWarehouseDto>? NewWarehouses { get; set; }
    }

    public class NewAuthorDto
    {
        // Temporary negative id used on the client to reference this new item before persistence.
        public int TempId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
    }

    public class NewNameDto
    {
        public int TempId { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class NewWarehouseDto
    {
        public int TempId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Street { get; set; }
        public string? Number { get; set; }
        public string? City { get; set; }
        public string? ZipCode { get; set; }
    }

    // New DTOs for form option lists
    public class SelectItemDto { public int Id { get; set; } public string Name { get; set; } = string.Empty; }

    public class FormOptionsDto
    {
        public List<SelectItemDto> Authors { get; set; } = new();
        public List<SelectItemDto> Genres { get; set; } = new();
        public List<SelectItemDto> Kinds { get; set; } = new();
        public List<SelectItemDto> Periods { get; set; } = new();
        public List<SelectItemDto> Warehouses { get; set; } = new();
    }
}

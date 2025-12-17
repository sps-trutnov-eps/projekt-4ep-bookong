using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bookong.Application.DTOs
{
    public class CreateBookDto : IValidatableObject
    {
        [Required(ErrorMessage = "Zadejte název knihy.")]
        public string? Title { get; set; }

        public int? AuthorId { get; set; }
        public bool SkipAuthor { get; set; }

        public string? ISBN { get; set; }
        public bool SkipISBN { get; set; }

        public int? GenreId { get; set; }
        public bool SkipGenre { get; set; }

        public int? KindId { get; set; }
        public bool SkipKind { get; set; }

        public int? PeriodId { get; set; }
        public bool SkipPeriod { get; set; }

        // Use int? here so Blazor's InputNumber binds to a supported type. Will be cast to ushort when creating domain entity.
        [Range(1, 10000, ErrorMessage = "Počet stran musí být mezi 1 a 10 000.")]
        public int? Pages { get; set; }
        public bool SkipPages { get; set; }

        public int? PublisherId { get; set; }
        public bool SkipPublisher { get; set; }

        public DateTime? DateRelease { get; set; }
        public bool SkipDateRelease { get; set; }

        public int? WarehouseId { get; set; }
        public bool SkipWarehouse { get; set; }

        public bool Borrowable { get; set; } = true;

        // Collections for new related entities created inline in the form.
        // Each new item carries a TempId (negative) which matches the temporary select value used in the UI.
        public List<NewAuthorDto>? NewAuthors { get; set; }
        public List<NewNameDto>? NewGenres { get; set; }
        public List<NewNameDto>? NewKinds { get; set; }
        public List<NewNameDto>? NewPeriods { get; set; }
        public List<NewPublisherDto>? NewPublishers { get; set; }
        public List<NewWarehouseDto>? NewWarehouses { get; set; }

        // Conditional validation: when a "Skip" flag is false, the corresponding value must be provided.
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!SkipAuthor && AuthorId == null)
            {
                yield return new ValidationResult("Vyberte autora nebo zaškrtněte 'Nevyplňovat'.", new[] { nameof(AuthorId) });
            }

            if (!SkipISBN && string.IsNullOrWhiteSpace(ISBN))
            {
                yield return new ValidationResult("Zadejte ISBN nebo zaškrtněte 'Nevyplňovat'.", new[] { nameof(ISBN) });
            }

            if (!SkipGenre && GenreId == null)
            {
                yield return new ValidationResult("Vyberte žánr nebo zaškrtněte 'Nevyplňovat'.", new[] { nameof(GenreId) });
            }

            if (!SkipKind && KindId == null)
            {
                yield return new ValidationResult("Vyberte literární druh nebo zaškrtněte 'Nevyplňovat'.", new[] { nameof(KindId) });
            }

            if (!SkipPeriod && PeriodId == null)
            {
                yield return new ValidationResult("Vyberte historické období nebo zaškrtněte 'Nevyplňovat'.", new[] { nameof(PeriodId) });
            }

            if (!SkipPages && Pages == null)
            {
                yield return new ValidationResult("Zadejte počet stran nebo zaškrtněte 'Nevyplňovat'.", new[] { nameof(Pages) });
            }

            if (!SkipPublisher && PublisherId == null)
            {
                yield return new ValidationResult("Vyberte vydavatele nebo zaškrtněte 'Nevyplňovat'.", new[] { nameof(PublisherId) });
            }

            if (!SkipDateRelease && DateRelease == null)
            {
                yield return new ValidationResult("Zadejte datum vydání nebo zaškrtněte 'Nevyplňovat'.", new[] { nameof(DateRelease) });
            }

            if (!SkipWarehouse && WarehouseId == null)
            {
                yield return new ValidationResult("Vyberte sklad nebo zaškrtněte 'Nevyplňovat'.", new[] { nameof(WarehouseId) });
            }
        }
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

    public class NewPublisherDto
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
        public List<SelectItemDto> Publishers { get; set; } = new();
        public List<SelectItemDto> Warehouses { get; set; } = new();

    }
}

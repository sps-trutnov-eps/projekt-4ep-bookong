namespace Bookong.Domain.Entities
{
    public class Publisher
    {
        public int Id { get; set; }
        public required Guid PublicId { get; set; } = Guid.NewGuid();

        public required string Name { get; set; }


        /**
         * Optional fields
         */
        public string? Description { get; set; }

        public string? RegistrationNumber { get; set; }

        public string? TaxId { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Website { get; set; }
    }
}

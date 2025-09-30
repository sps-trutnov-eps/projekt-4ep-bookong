namespace Bookong.Application.DTOs
{
    public class ImportBookDto
    {
        public required string Title { get; set; }

        public string? ISBN { get; set; }

        public required string AuthorName { get; set; }

        public required string GenreName { get; set; }

        public required string KindName { get; set; }

        public required string PeriodName { get; set; }

        public required ushort Pages { get; set; }

        public string? PublisherName { get; set; }

        public DateTime? DateRelease { get; set; }

        public required string WarehouseName { get; set; }
        }
}

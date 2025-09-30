namespace Bookong.Application.DTOs
{
    public class BookDetailDto
    {
        public Guid PublicId { get; set; }

        public string Title { get; set; } = "";

        public Guid AuthorId { get; set; }
        public string AuthorFullName { get; set; } = "";

        public string ISBN { get; set; } = "";

        public ushort Pages { get; set; }

        public int GenreId { get; set; }
        public string GenreName { get; set; } = "";

        public int KindId { get; set; }
        public string KindName { get; set; } = "";

        public int PeriodId { get; set; }
        public string PeriodName { get; set; } = "";

        public int? PublisherId { get; set; }
        public string? PublisherName { get; set; }

        public DateTime? DateRelease { get; set; }

        public int? WarehouseId { get; set; }
        public string? WarehouseName { get; set; }
    }
}

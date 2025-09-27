namespace Bookong.Application.DTOs
{
    public class BookExportDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string Author { get; set; } = "";
        public string ISBN { get; set; } = "";
        public string Genre { get; set; } = "";
        public string Kind { get; set; } = "";
        public string Period { get; set; } = "";
        public int Pages { get; set; }
        public DateTime? DateRelease { get; set; }
        public string Warehouse { get; set; } = "";
    }
}

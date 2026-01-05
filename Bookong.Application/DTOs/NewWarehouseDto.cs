namespace Bookong.Application.DTOs
{
    public class NewWarehouseDto
    {
        public int TempId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Street { get; set; }
        public string? Number { get; set; }
        public string? City { get; set; }
        public string? ZipCode { get; set; }
    }
}

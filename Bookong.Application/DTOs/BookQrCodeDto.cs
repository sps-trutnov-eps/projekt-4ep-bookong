namespace Bookong.Application.DTOs
{
 public class BookQrCodeDto
 {
        public int InternalId { get; set; }
        public Guid PublicId { get; set; }
        public string Name { get; set; } = "";
        public byte[] QrCodeImage { get; set; } = [];
    }
}

namespace Bookong.Application.DTOs
{
    public class BookQrCodeRequestDto
    {
        public Guid BookPublicId { get; set; }
        public int SizePx { get; set; } = 250;
    }
}

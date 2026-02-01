namespace Bookong.Application.DTOs
{
    public class CreateBookReservationDto
    {
        public Guid BookPublicId { get; set; }

        public Guid UserPublicId { get; set; }
    }
}

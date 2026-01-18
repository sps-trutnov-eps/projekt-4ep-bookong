using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Queries.Interfaces;
using Bookong.Domain.Interfaces;

namespace Bookong.Application.UseCases.Queries
{
    public class GetBookDetailQuery : IGetBookDetailQuery
    {
        private readonly IBookRepository _books;

        public GetBookDetailQuery(IUnitOfWork unitOfWork)
        {
            _books = unitOfWork.Books;
        }

        public async Task<BookDetailDto?> ExecuteAsync(Guid publicId)
        {
            var book = await _books.GetByPublicIdAsync(publicId);
            if (book == null)
            {
                return null;
            }

            return new BookDetailDto
            {
                PublicId = book.PublicId,
                Title = book.Name,
                AuthorId = Guid.Empty, // Note: AuthorId in DTO is Guid, but in entity it's int? - this might need adjustment
                AuthorFullName = book.Author != null ? $"{book.Author.FirstName} {book.Author.LastName}" : "",
                ISBN = book.ISBN ?? "",
                Pages = book.Pages ?? 0,
                GenreId = book.GenreId ?? 0,
                GenreName = book.Genre?.Name ?? "",
                KindId = book.KindId ?? 0,
                KindName = book.Kind?.Name ?? "",
                PeriodId = book.PeriodId ?? 0,
                PeriodName = book.Period?.Name ?? "",
                PublisherId = book.PublisherId,
                PublisherName = book.Publisher?.Name,
                DateRelease = book.DateRelease,
                WarehouseId = book.WarehouseId,
                WarehouseName = book.Warehouse?.Name
            };
        }
    }
}

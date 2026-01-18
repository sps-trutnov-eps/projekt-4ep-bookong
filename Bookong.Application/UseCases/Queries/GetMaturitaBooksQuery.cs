using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Queries.Interfaces;
using Bookong.Domain.Interfaces;

namespace Bookong.Application.UseCases.Queries
{
    public class GetMaturitaBooksQuery : IGetMaturitaBooksQuery
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMaturitaBooksQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<BookDetailDto>> ExecuteAsync()
        {
            var maturitaBooks = await _unitOfWork.MaturitaBooks.GetAllWithDetailsAsync();

            return maturitaBooks.Select(mb => new BookDetailDto
            {
                PublicId = mb.PublicId,
                Title = mb.Name,
                AuthorId = mb.Author?.PublicId ?? Guid.Empty,
                AuthorFullName = mb.Author != null 
                    ? $"{mb.Author.FirstName} {mb.Author.LastName}".Trim() 
                    : "",
                ISBN = "",
                Pages = 0,
                GenreId = mb.Genre?.Id ?? 0,
                GenreName = mb.Genre?.Name ?? "",
                KindId = mb.Kind?.Id ?? 0,
                KindName = mb.Kind?.Name ?? "",
                PeriodId = mb.Period?.Id ?? 0,
                PeriodName = mb.Period?.Name ?? "",
                PublisherId = null,
                PublisherName = null,
                DateRelease = null,
                WarehouseId = null,
                WarehouseName = null
            });
        }
    }
}

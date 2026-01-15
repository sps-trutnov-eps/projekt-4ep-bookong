using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Queries.Interfaces;
using Bookong.Domain.Interfaces;

namespace Bookong.Application.UseCases.Queries
{
    public class GetAllBooksQuery : IGetAllBooksQuery
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllBooksQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<BookListItemDto>> ExecuteAsync()
        {
            var books = await _unitOfWork.Books.GetAllAsync();

            return books.Select(b => new BookListItemDto
            {
                PublicId = b.PublicId,
                Title = b.Name ?? string.Empty,
                AuthorFullName = b.Author != null
                    ? string.Join(' ', new[] { b.Author.FirstName, b.Author.MiddleName, b.Author.LastName }
                        .Where(s => !string.IsNullOrWhiteSpace(s)))
                    : string.Empty,
                Genre = b.Genre?.Name ?? string.Empty,
                Kind = b.Kind?.Name ?? string.Empty,
                Borrowable = b.Borrowable
            }).ToList();
        }
    }
}
using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Queries.Interfaces;
using Bookong.Domain.Interfaces;
using Bookong.Infrastructure.Services;

namespace Bookong.Application.UseCases.Queries
{
    public class GetAvailableBooksQuery : IGetAvailableBooksQuery
    {
        private readonly IBookRepository _books;
        public GetAvailableBooksQuery(IUnitOfWork unitOfWork)
        {
            _books = unitOfWork.Books;
        }

        public async Task<IReadOnlyList<BookListItemDto>> ExecuteAsync()
        {
            var all = await _books.GetAllAsync();
            return all
                .Where(b => b.Borrowable)
                .Select(b => new BookListItemDto
                {
                    PublicId = b.PublicId,
                    Title = b.Name,
                    AuthorFullName = $"{b.Author.FirstName} {b.Author.LastName}",
                    Genre = b.Genre.Name,
                    Kind = b.Kind.Name,
                    Borrowable = b.Borrowable
                })
                .ToList();
        }
    }
}

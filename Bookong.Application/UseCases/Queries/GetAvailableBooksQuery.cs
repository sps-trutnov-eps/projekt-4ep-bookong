using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Queries.Interfaces;
using Bookong.Domain.Interfaces;

namespace Bookong.Application.UseCases.Queries
{
    public class GetAvailableBooksQuery : IGetAvailableBooksQuery
    {
        private readonly IUnitOfWork _uow;

        public GetAvailableBooksQuery(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IReadOnlyList<BookListItemDto>> ExecuteAsync()
        {
            var books = await _uow.Books.GetAllAsync();
            var allLoans = await _uow.BookLoans.GetAllAsync();

            var loansByBookId = allLoans
                .GroupBy(bl => bl.BookId)
                .ToDictionary(g => g.Key, g => g.Count());

            var result = books
                .Where(b => b.Borrowable)
                .Select(b => new BookListItemDto
                {
                    PublicId = b.PublicId,
                    Title = b.Name,
                    AuthorFullName = b.Author != null 
                        ? $"{b.Author.FirstName} {b.Author.LastName}" 
                        : "Neznámý autor",
                    Genre = b.Genre?.Name ?? "Nezařazeno",
                    Kind = b.Kind?.Name ?? "Nezařazeno",
                    Borrowable = b.Borrowable
                })
                .ToList();

            return result;
        }
    }
}

using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Queries.Interfaces;
using Bookong.Domain.Interfaces;
using Bookong.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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
            // Fix: IBookRepository does not support Include/Where/Select directly.
            // Use GetAllAsync() and project manually.
            var books = await _uow.Books.GetAllAsync();

            var result = books
                .Where(b => b.Borrowable)
                .Select(b => new BookListItemDto
                {
                    InternalId = b.Id,
                    PublicId = b.PublicId,
                    Title = b.Name,
                    AuthorFullName = $"{b.Author.FirstName} {b.Author.LastName}",
                    Genre = b.Genre.Name,
                    Kind = b.Kind.Name,
                    Borrowable = b.Borrowable
                })
                .ToList();

            return result;
        }
    }
}

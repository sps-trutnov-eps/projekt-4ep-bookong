using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Queries.Interfaces;
using Bookong.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bookong.Application.UseCases.Queries
{
    public class GetAvailableBooksQuery : IGetAvailableBooksQuery
    {
        private readonly BookongDbContext _dbContext;
        
        public GetAvailableBooksQuery(BookongDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<BookListItemDto>> ExecuteAsync()
        {
            var books = await _dbContext.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .Include(b => b.Kind)
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
                .ToListAsync();
            
            return books;
        }
    }
}

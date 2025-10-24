using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Queries.Interfaces;
using Bookong.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bookong.Application.UseCases.Queries
{
    public class GetMaturitaBookSelectionQuery : IGetMaturitaBookSelectionQuery
    {
        private readonly BookongDbContext _dbContext;
        private readonly ILogger<GetMaturitaBookSelectionQuery> _logger;

        public GetMaturitaBookSelectionQuery(BookongDbContext dbContext, ILogger<GetMaturitaBookSelectionQuery> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IEnumerable<BookListItemDto>> ExecuteAsync()
        {
            try
            {
                // Get the first user from seeded data
                var user = await _dbContext.Users.FirstOrDefaultAsync()
                    ?? throw new InvalidOperationException("No users found in database. Please run seed data first.");

                _logger.LogInformation("Fetching maturita book selection for user {UserId}", user.Id);

                return await _dbContext.MaturitaBookSelections
                    .Include(m => m.Book)
                    .ThenInclude(b => b.Author)
                    .Include(m => m.Book.Genre)
                    .Include(m => m.Book.Kind)
                    .Where(m => m.UserId == user.Id)
                    .Select(m => new BookListItemDto
                    {
                        PublicId = m.Book.PublicId,
                        Title = m.Book.Name,
                        AuthorFullName = m.Book.Author.FirstName + " " + m.Book.Author.LastName,
                        Genre = m.Book.Genre.Name,
                        Kind = m.Book.Kind.Name,
                        Borrowable = m.Book.Borrowable
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching maturita book selection");
                throw;
            }
        }
    }
}
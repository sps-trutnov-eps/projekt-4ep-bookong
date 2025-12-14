using Bookong.Application.UseCases.Queries.Interfaces;
using Bookong.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bookong.Application.UseCases.Queries
{
    public class GetMaturitaBooksQuery : IGetMaturitaBooksQuery
    {
        private readonly BookongDbContext _dbContext;

        public GetMaturitaBooksQuery(BookongDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<string>> ExecuteAsync()
        {
            var maturitaBooks = await _dbContext.MaturitaBooks
                .AsNoTracking()
                .Include(mb => mb.Author)
                .Include(mb => mb.Genre)
                .Include(mb => mb.Kind)
                .ToListAsync();

            return maturitaBooks.Select(mb =>
                GetBookKey(mb.Name, mb.Author?.FirstName, mb.Author?.LastName, mb.Genre?.Name, mb.Kind?.Name));
        }

        private static string GetBookKey(string? title, string? authorFirst, string? authorLast, string? genre, string? kind)
        {
            return $"{title}|{authorFirst}|{authorLast}|{genre}|{kind}".ToLowerInvariant();
        }
    }
}
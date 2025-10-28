using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Queries.Interfaces;
using Bookong.Domain.Interfaces;
using Bookong.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bookong.Application.UseCases.Queries
{
    public class GetLibraryStatisticsQuery : IGetLibraryStatisticsQuery
    {
        private readonly IUnitOfWork _uow;

        public GetLibraryStatisticsQuery(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<LibraryStatisticsDto> ExecuteAsync()
        {
            var totalUsersNullable = await _uow.Users.CountAsync();
            var totalUsers = totalUsersNullable ?? 0;

            var totalBooksInLibrary = await _uow.Books.CountAsync();

            // Load all book loans with related data eagerly
            var bookLoans = (await _uow.BookLoans.GetAllAsync()).ToList();

            var activeLoans = bookLoans.Count(bl => bl.ReturnDate == null);
            var totalLoansAllTime = bookLoans.Count;

            // Most popular book (most borrowed)
            var mostPopularBookGroup = bookLoans
                .GroupBy(bl => bl.Book?.Name)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault();
            var mostPopularBook = mostPopularBookGroup?.Key;

            // Most popular author
            var mostPopularAuthorGroup = bookLoans
                .GroupBy(bl => $"{bl.Book?.Author?.FirstName} {bl.Book?.Author?.LastName}")
                .OrderByDescending(g => g.Count())
                .FirstOrDefault();
            var mostPopularAuthor = mostPopularAuthorGroup?.Key;

            // Most popular genre
            var mostPopularGenreGroup = bookLoans
                .GroupBy(bl => bl.Book?.Genre?.Name)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault();
            var mostPopularGenre = mostPopularGenreGroup?.Key;

            // Top 5 books in the last month
            var oneMonthAgo = DateTime.UtcNow.AddMonths(-1);
            var topBooksLastMonth = bookLoans
                .Where(bl => bl.LoanDate >= oneMonthAgo)
                .GroupBy(bl => new { bl.Book?.Name, AuthorName = $"{bl.Book?.Author?.FirstName} {bl.Book?.Author?.LastName}" })
                .Select(g => new BookStatDto
                {
                    BookTitle = g.Key.Name,
                    AuthorName = g.Key.AuthorName,
                    LoanCount = g.Count()
                })
                .OrderByDescending(b => b.LoanCount)
                .Take(5)
                .ToList();

            // Loans this month vs last month
            var thisMonthStart = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            var lastMonthStart = thisMonthStart.AddMonths(-1);

            var loansThisMonth = bookLoans.Count(bl => bl.LoanDate >= thisMonthStart);
            var loansLastMonth = bookLoans.Count(bl => bl.LoanDate >= lastMonthStart && bl.LoanDate < thisMonthStart);

            // Monthly trend for last 6 months
            var monthlyTrend = new Dictionary<string, int>();
            for (int i = 5; i >= 0; i--)
            {
                var monthStart = DateTime.UtcNow.AddMonths(-i).Date;
                monthStart = new DateTime(monthStart.Year, monthStart.Month, 1);
                var monthEnd = monthStart.AddMonths(1);
                var monthName = monthStart.ToString("MMM yyyy");
                var count = bookLoans.Count(bl => bl.LoanDate >= monthStart && bl.LoanDate < monthEnd);
                monthlyTrend[monthName] = count;
            }

            return new LibraryStatisticsDto
            {
                TotalUsers = totalUsers,
                ActiveLoans = activeLoans,
                TotalBooksInLibrary = totalBooksInLibrary,
                TotalLoansAllTime = totalLoansAllTime,
                MostPopularBook = mostPopularBook,
                MostPopularAuthor = mostPopularAuthor,
                MostPopularGenre = mostPopularGenre,
                TopBooksLastMonth = topBooksLastMonth,
                LoansThisMonth = loansThisMonth,
                LoansLastMonth = loansLastMonth,
                MonthlyLoanTrend = monthlyTrend
            };
        }
    }
}

namespace Bookong.Application.DTOs
{
    public class LibraryStatisticsDto
    {
        // Overall Library Stats
        public int TotalUsers { get; set; }
        public int ActiveLoans { get; set; }
        public int TotalBooksInLibrary { get; set; }
        public int TotalLoansAllTime { get; set; }
        
        // Popular Content
        public string? MostPopularBook { get; set; }
        public string? MostPopularAuthor { get; set; }
        public string? MostPopularGenre { get; set; }
        
        // Top 5 Books (Last Month)
        public List<BookStatDto> TopBooksLastMonth { get; set; } = new();
        
        // Trends
        public int LoansThisMonth { get; set; }
        public int LoansLastMonth { get; set; }
        public Dictionary<string, int> MonthlyLoanTrend { get; set; } = new();
    }

    public class BookStatDto
    {
        public string BookTitle { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public int LoanCount { get; set; }
    }
}

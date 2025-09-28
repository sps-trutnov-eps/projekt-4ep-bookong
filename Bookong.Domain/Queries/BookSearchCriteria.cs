namespace Bookong.Domain.Queries
{
    public record BookSearchCriteria
    (
        string? Title,
        int? AuthorId,
        int? GenreId,
        int? KindId,
        int? PeriodId,
        string? Publisher,

        int PageNumber = 1,
        int PageSize = 25,
        string SortBy = "Title"
    );
}

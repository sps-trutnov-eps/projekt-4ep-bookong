using System.Diagnostics.CodeAnalysis;

namespace Bookong.Domain.Common.Models
{
    public record PagedResult<T>
    {
        public required IEnumerable<T> Items { get; init; }

        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public int TotalPages { get; init; }
        public long TotalCount { get; init; }

        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

        [SetsRequiredMembers]
        public PagedResult(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
        {
            Items = items ?? [];
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }
}

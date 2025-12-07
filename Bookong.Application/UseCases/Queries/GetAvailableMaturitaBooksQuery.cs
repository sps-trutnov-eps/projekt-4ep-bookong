using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Queries.Interfaces;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Bookong.Application.UseCases.Queries
{
    public class GetMaturitaBooksQuery : IGetMaturitaBooksQuery
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetMaturitaBooksQuery> _logger;

        public GetMaturitaBooksQuery(IUnitOfWork unitOfWork, ILogger<GetMaturitaBooksQuery> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<BookDetailDto>> ExecuteAsync()
        {
            try
            {
                var paged = await _unitOfWork.MaturitaBooks.GetAllAsync();

                // defensive: if PagedResult uses different property name, log and return empty
                var items = paged?.Items as IEnumerable<MaturitaBook>;
                if (items == null)
                {
                    _logger.LogWarning("GetMaturitaBooksQuery: paged result or Items is null. Paged type: {Type}", paged?.GetType().FullName);
                    return Enumerable.Empty<BookDetailDto>();
                }

                return items.Select(mb => new BookDetailDto
                {
                    PublicId = mb.PublicId,
                    Title = mb.Name ?? string.Empty,
                    AuthorId = mb.Author?.PublicId ?? Guid.Empty,
                    AuthorFullName = mb.Author != null
                        ? string.Join(' ', new[] { mb.Author.FirstName, mb.Author.MiddleName, mb.Author.LastName }
                            .Where(s => !string.IsNullOrWhiteSpace(s)))
                        : string.Empty,
                    ISBN = string.Empty,
                    Pages = 0,
                    GenreId = mb.Genre?.Id ?? 0,
                    GenreName = mb.Genre?.Name ?? string.Empty,
                    KindId = mb.Kind?.Id ?? 0,
                    KindName = mb.Kind?.Name ?? string.Empty,
                    PeriodId = mb.Period?.Id ?? 0,
                    PeriodName = mb.Period?.Name ?? string.Empty,
                    PublisherId = null,
                    PublisherName = null,
                    DateRelease = null,
                    WarehouseId = null,
                    WarehouseName = null
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load maturita books in GetMaturitaBooksQuery.ExecuteAsync()");
                return Enumerable.Empty<BookDetailDto>();
            }
        }
    }

    // Adapter: expose legacy string-key interface by mapping BookDetailDto -> string key.
    public class GetAvailableMaturitaBooksQuery : IGetAvailableMaturitaBooksQuery
    {
        private readonly IGetMaturitaBooksQuery _inner;
        public GetAvailableMaturitaBooksQuery(IGetMaturitaBooksQuery inner) => _inner = inner;

        public async Task<IEnumerable<string>> ExecuteAsync()
        {
            var dtos = await _inner.ExecuteAsync();

            return dtos.Select(d =>
            {
                var names = (d.AuthorFullName ?? string.Empty)
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var first = names.FirstOrDefault() ?? string.Empty;
                var last = names.Length > 1 ? names.Last() : string.Empty;

                var key = $"{d.Title}|{first}|{last}|{d.GenreName}|{d.KindName}";
                return key.ToLowerInvariant();
            }).ToList();
        }
    }
}
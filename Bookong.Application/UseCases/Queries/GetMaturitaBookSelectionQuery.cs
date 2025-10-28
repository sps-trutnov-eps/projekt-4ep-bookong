using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Queries.Interfaces;
using Bookong.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Bookong.Application.UseCases.Queries
{
    public class GetMaturitaBookSelectionQuery : IGetMaturitaBookSelectionQuery
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetMaturitaBookSelectionQuery> _logger;

        public GetMaturitaBookSelectionQuery(IUnitOfWork unitOfWork, ILogger<GetMaturitaBookSelectionQuery> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<BookListItemDto>> ExecuteAsync()
        {
            try
            {
                // Get the first user from seeded data
                var users = await _unitOfWork.Users.GetAllAsync();
                var user = users.FirstOrDefault()
                    ?? throw new InvalidOperationException("No users found in database. Please run seed data first.");

                _logger.LogInformation("Fetching maturita book selection for user {UserId}", user.Id);

                var selections = await _unitOfWork.MaturitaBookSelections.GetByUserWithDetailsAsync(user.Id);

                if (selections == null)
                    return Enumerable.Empty<BookListItemDto>();

                var result = selections
                    .Select(m => new BookListItemDto
                    {
                        PublicId = m.Book.PublicId,
                        Title = m.Book.Name,
                        AuthorFullName = m.Book.Author.FirstName + " " + m.Book.Author.LastName,
                        Genre = m.Book.Genre.Name,
                        Kind = m.Book.Kind.Name,
                        Borrowable = m.Book.Borrowable
                    })
                    .ToList();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching maturita book selection");
                throw;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bookong.Application.DTOs;
using Bookong.Application.Services.Interfaces;
using Bookong.Application.UseCases.Queries.Interfaces;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Bookong.Application.UseCases.Queries
{
    public class GetMaturitaBookSelectionQuery : IGetMaturitaBookSelectionQuery
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<GetMaturitaBookSelectionQuery> _logger;

        public GetMaturitaBookSelectionQuery(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            ILogger<GetMaturitaBookSelectionQuery> logger)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<IEnumerable<BookListItemDto>> ExecuteAsync()
        {
            try
            {
                // Get current authenticated user (if present)
                User? user = null;
                try
                {
                    var currentUserPublicId = await _currentUserService.GetCurrentUserPublicIdAsync();
                    _logger.LogDebug("Current user PublicId = {CurrentUserPublicId}", currentUserPublicId);

                    if (currentUserPublicId.HasValue)
                    {
                        user = await _unitOfWork.Users.GetByPublicIdAsync(currentUserPublicId.Value);
                        if (user == null)
                        {
                            _logger.LogDebug("User with PublicId {PublicId} not found, will fallback to seeded user.", currentUserPublicId);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error reading current user identity, will fallback to seeded user.");
                }

                // Fallback to seeded user if no user in context
                if (user is null)
                {
                    var users = await _unitOfWork.Users.GetAllAsync();
                    user = users.FirstOrDefault()
                        ?? throw new InvalidOperationException("No users found in database. Please run seed data first.");
                    _logger.LogDebug("Using fallback seeded user Id={UserId}", user.Id);
                }

                _logger.LogInformation("Fetching maturita book selection for user {UserId}", user.Id);

                var selections = (await _unitOfWork.MaturitaBookSelections.GetByUserWithDetailsAsync(user.Id)).ToList();

                if (selections.Count == 0)
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

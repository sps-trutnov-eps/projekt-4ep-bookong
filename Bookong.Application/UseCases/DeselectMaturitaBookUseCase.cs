using System;
using System.Linq;
using System.Threading.Tasks;
using Bookong.Application.DTOs;
using Bookong.Application.Services.Interfaces;
using Bookong.Application.UseCases.Interfaces;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Bookong.Application.UseCases
{
    public class DeselectMaturitaBookUseCase : IDeselectMaturitaBookUseCase
    {
      private readonly IUnitOfWork _unitOfWork;
        private readonly ISessionService _sessionService;
        private readonly ILogger<DeselectMaturitaBookUseCase> _logger;

        public DeselectMaturitaBookUseCase(
            IUnitOfWork unitOfWork,
      ISessionService sessionService,
          ILogger<DeselectMaturitaBookUseCase> logger)
  {
   _unitOfWork = unitOfWork;
   _sessionService = sessionService;
            _logger = logger;
        }

        public async Task<GenericResponse> Handle(RemoveBookFromPersonalMaturitaListDto dto)
        {
            try
            {
                // Get user from session (if present)
    User? user = null;
          try
       {
        var currentUserPublicId = await _sessionService.GetAsync("CurrentUserPublicId");
                    _logger.LogDebug("Session CurrentUserPublicId = {CurrentUserPublicId}", currentUserPublicId);

      if (!string.IsNullOrWhiteSpace(currentUserPublicId) && Guid.TryParse(currentUserPublicId, out var publicId))
        {
       user = await _unitOfWork.Users.GetByPublicIdAsync(publicId);
                if (user == null)
      {
  _logger.LogDebug("User with PublicId {PublicId} not found, will fallback to seeded user.", publicId);
   }
          }
    }
      catch (Exception ex)
       {
   _logger.LogWarning(ex, "Error reading CurrentUserPublicId from session, will fallback to seeded user.");
        }

   // Fallback to seeded user if no user in session
if (user is null)
   {
var users = await _unitOfWork.Users.GetAllAsync();
            user = users.FirstOrDefault();
                    
    if (user is null)
           {
                    _logger.LogError("No users found in database. Please run seed data first.");
      return GenericResponse.FailureResponse("No users found in database. Please run seed data first.", "USER_NOT_FOUND");
           }

    _logger.LogDebug("Using fallback seeded user Id={UserId}", user.Id);
     }

         // Get the book by its public ID
    var book = await _unitOfWork.Books.GetByPublicIdAsync(dto.BookPublicId);
        
      if (book is null)
       {
     _logger.LogWarning("Book with PublicId {BookPublicId} not found", dto.BookPublicId);
         return GenericResponse.FailureResponse($"Book with PublicId {dto.BookPublicId} not found", "BOOK_NOT_FOUND");
                }

   // Find the matching selection
    var selections = await _unitOfWork.MaturitaBookSelections.GetAllAsync();
        var selection = selections.FirstOrDefault(m => m.BookId == book.Id && m.UserId == user.Id);

 if (selection is null)
     {
    _logger.LogInformation("Book {BookId} was not found in maturita selection for user {UserId}", book.Id, user.Id);
return GenericResponse.SuccessResponse("Book is not in your maturita selection");
         }

        _logger.LogInformation("Removing book {BookId} from maturita selection for user {UserId}", book.Id, user.Id);

    _unitOfWork.MaturitaBookSelections.Delete(selection);
       await _unitOfWork.CommitAsync();

    _logger.LogInformation("Book {BookId} successfully removed from maturita selection for user {UserId}", book.Id, user.Id);
           return GenericResponse.SuccessResponse("Book successfully removed from your maturita selection");
            }
catch (Exception ex)
    {
    _logger.LogError(ex, "Error removing book {BookPublicId} from maturita selection", dto.BookPublicId);
     return GenericResponse.FailureResponse("An error occurred while removing the book from your maturita selection", "INTERNAL_ERROR");
    }
        }
    }
}

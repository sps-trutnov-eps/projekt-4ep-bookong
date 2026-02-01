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
    public class SelectMaturitaBookUseCase : ISelectMaturitaBookUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<SelectMaturitaBookUseCase> _logger;

        public SelectMaturitaBookUseCase(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            ILogger<SelectMaturitaBookUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<GenericResponse> Handle(AddBookToPersonalMaturitaListDto dto)
        {
            try
            {
                User? user = null;
                try
                {
                    var currentUserPublicId = await _currentUserService.GetCurrentUserPublicIdAsync();
                    if (currentUserPublicId.HasValue)
                    {
                        user = await _unitOfWork.Users.GetByPublicIdAsync(currentUserPublicId.Value);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error reading current user identity, will fallback.");
                }

                if (user is null)
                {
                    var users = await _unitOfWork.Users.GetAllAsync();
                    user = users.FirstOrDefault();

                    if (user is null)
                    {
                        user = new User
                        {
                            PublicId = Guid.NewGuid(),
                            SamAccountName = "local.user",
                            ObjectGuid = Guid.NewGuid(),
                            Name = "Lokální uživatel",
                            Email = "local@bookong.local",
                            OrganizationalUnit = "OU=Local,DC=bookong,DC=local"
                        };
                        _unitOfWork.Users.Add(user);
                        await _unitOfWork.CommitAsync();
                    }
                }

                var maturitaBook = await _unitOfWork.MaturitaBooks.GetByPublicIdAsync(dto.BookPublicId);
                if (maturitaBook is null)
                {
                    return GenericResponse.FailureResponse("Maturita book not found", "BOOK_NOT_FOUND");
                }

                var books = await _unitOfWork.Books.GetAllAsync();
                var book = books.FirstOrDefault(b => 
                    b.Name == maturitaBook.Name && 
                    b.AuthorId == maturitaBook.AuthorId);

                if (book is null)
                {
                    book = new Book
                    {
                        PublicId = Guid.NewGuid(),
                        Name = maturitaBook.Name,
                        AuthorId = maturitaBook.AuthorId,
                        GenreId = maturitaBook.GenreId,
                        KindId = maturitaBook.KindId,
                        PeriodId = maturitaBook.PeriodId,
                        Borrowable = false
                    };
                    _unitOfWork.Books.Add(book);
                    await _unitOfWork.CommitAsync();
                }

                var allSelections = await _unitOfWork.MaturitaBookSelections.GetAllAsync();
                var exists = allSelections.Any(m => m.BookId == book.Id && m.UserId == user.Id);

                if (exists)
                {
                    return GenericResponse.SuccessResponse("Book is already in your maturita selection");
                }

                var selection = new MaturitaBookSelection
                {
                    BookId = book.Id,
                    UserId = user.Id
                };

                _unitOfWork.MaturitaBookSelections.Add(selection);
                await _unitOfWork.CommitAsync();

                return GenericResponse.SuccessResponse("Book successfully added to your maturita selection");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding book to maturita selection");
                return GenericResponse.FailureResponse("An error occurred while adding the book", "INTERNAL_ERROR");
            }
        }
    }
}

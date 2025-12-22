using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Interfaces;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;

namespace Bookong.Application.UseCases
{
    public class CreateBookLoanUseCase : ICreateBookLoanUseCase
    {
        private readonly IUnitOfWork _uow;

        public CreateBookLoanUseCase(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<GenericResponse> ExecuteAsync(CreateBookLoanDto dto)
        {
            try
            {
                var book = await _uow.Books.GetByPublicIdAsync(dto.BookPublicId);
                if (book == null)
                    return GenericResponse.FailureResponse("Kniha nebyla nalezena.");

                var user = await _uow.Users.GetByPublicIdAsync(dto.PublicId);
                if (user == null)
                    return GenericResponse.FailureResponse("Uživatel nebyl nalezen.");

                var existingLoans = await _uow.BookLoans.GetAllAsync();
                var activeLoan = existingLoans.FirstOrDefault(bl => 
                    bl.BookId == book.Id && bl.ReturnDate == null);

                if (activeLoan != null)
                    return GenericResponse.FailureResponse("Tato kniha je již vypůjčena.");

                var bookLoan = new BookLoan
                {
                    BookId = book.Id,
                    Book = null!,
                    UserId = user.Id,
                    User = null!,
                    LoanDate = DateTime.UtcNow
                };

                _uow.BookLoans.Add(bookLoan);
                await _uow.CommitAsync();

                return GenericResponse.SuccessResponse($"Kniha '{book.Name}' byla úspěšně vypůjčena uživateli {user.Name}.");
            }
            catch (Exception ex)
            {
                return GenericResponse.FailureResponse($"Chyba při vytváření výpůjčky: {ex.Message}");
            }
        }
    }
}

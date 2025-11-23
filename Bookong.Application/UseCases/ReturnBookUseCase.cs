using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Interfaces;
using Bookong.Domain.Interfaces;

namespace Bookong.Application.UseCases
{
    public class ReturnBookUseCase : IReturnBookUseCase
    {
        private readonly IUnitOfWork _uow;

        public ReturnBookUseCase(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<GenericResponse> ExecuteAsync(Guid bookPublicId)
        {
            try
            {
                var book = await _uow.Books.GetByPublicIdAsync(bookPublicId);
                if (book == null)
                    return GenericResponse.FailureResponse("Kniha nebyla nalezena.");

                var allLoans = await _uow.BookLoans.GetAllAsync();
                var activeLoan = allLoans.FirstOrDefault(bl => 
                    bl.BookId == book.Id && bl.ReturnDate == null);

                if (activeLoan == null)
                    return GenericResponse.FailureResponse("Tato kniha není aktuálně vypůjčena.");

                activeLoan.ReturnDate = DateTime.UtcNow;
                _uow.BookLoans.Update(activeLoan);
                await _uow.CommitAsync();

                return GenericResponse.SuccessResponse($"Kniha '{book.Name}' byla úspěšně vrácena.");
            }
            catch (Exception ex)
            {
                return GenericResponse.FailureResponse($"Chyba při vracení knihy: {ex.Message}");
            }
        }
    }
}

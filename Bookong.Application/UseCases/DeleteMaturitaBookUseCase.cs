using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Interfaces;
using Bookong.Domain.Interfaces;

namespace Bookong.Application.UseCases
{
    public class DeleteMaturitaBookUseCase : IDeleteMaturitaBookUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteMaturitaBookUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse> ExecuteAsync(Guid bookPublicId)
        {
            var book = await _unitOfWork.Books.GetByPublicIdAsync(bookPublicId);
            if (book == null)
                return GenericResponse.FailureResponse("Kniha nebyla nalezena.");

            if (book.Author == null || book.Genre == null || book.Kind == null || book.Period == null)
                return GenericResponse.FailureResponse("Kniha nemá všechny požadované údaje.");

            var existing = await _unitOfWork.MaturitaBooks.FindExistingAsync(
                book.Name,
                book.Author.Id,
                book.Genre.Id,
                book.Kind.Id,
                book.Period.Id);

            if (existing == null)
                return GenericResponse.FailureResponse("Kniha není v dostupných.");

            // Remove any user selections that reference physical books matching this maturita book
            try
            {
                var allBooks = await _unitOfWork.Books.GetAllAsync();
                var matchingBookIds = allBooks
                    .Where(b => string.Equals(b.Name, book.Name, StringComparison.OrdinalIgnoreCase)
                            && b.AuthorId == book.AuthorId
                            && b.GenreId == book.GenreId
                            && b.KindId == book.KindId
                            && b.PeriodId == book.PeriodId)
                    .Select(b => b.Id)
                    .ToHashSet();

                if (matchingBookIds.Any())
                {
                    var selections = await _unitOfWork.MaturitaBookSelections.GetAllAsync();
                    var toRemove = selections.Where(s => matchingBookIds.Contains(s.BookId)).ToList();

                    foreach (var sel in toRemove)
                    {
                        _unitOfWork.MaturitaBookSelections.Delete(sel);
                    }
                }

                // Delete maturita book entry
                _unitOfWork.MaturitaBooks.Delete(existing);
                await _unitOfWork.CommitAsync();

                return GenericResponse.SuccessResponse("Kniha byla odebrána z dostupných a ze seznamů uživatelů, kde byla vybrána.");
            }
            catch (Exception)
            {
                return GenericResponse.FailureResponse("Došlo k chybě při odebírání knihy.");
            }
        }
    }
}
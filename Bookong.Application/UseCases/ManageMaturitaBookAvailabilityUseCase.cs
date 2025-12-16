using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Interfaces;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;

namespace Bookong.Application.UseCases
{
    public class ManageMaturitaBookAvailabilityUseCase : ICreateMaturitaBookUseCase, IDeleteMaturitaBookUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ManageMaturitaBookAvailabilityUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse> AddToAvailableAsync(Guid bookPublicId)
        {
            var book = await _unitOfWork.Books.GetByPublicIdAsync(bookPublicId);
            if (book == null)
                return GenericResponse.FailureResponse("Kniha nebyla nalezena.");

            if (book.Author == null || book.Genre == null || book.Kind == null || book.Period == null)
                return GenericResponse.FailureResponse("Kniha nemá všechny požadované údaje.");

            // Check if already exists
            var existing = await _unitOfWork.MaturitaBooks.FindExistingAsync(
                book.Name,
                book.Author.Id,
                book.Genre.Id,
                book.Kind.Id,
                book.Period.Id);

            if (existing != null)
                return GenericResponse.FailureResponse("Kniha již je v dostupných.");

            var maturitaBook = new MaturitaBook
            {
                Name = book.Name,
                AuthorId = book.Author.Id,
                GenreId = book.Genre.Id,
                KindId = book.Kind.Id,
                PeriodId = book.Period.Id
            };

            _unitOfWork.MaturitaBooks.Add(maturitaBook);
            await _unitOfWork.CommitAsync();

            return GenericResponse.SuccessResponse("Kniha byla přidána do dostupných.");
        }

        public async Task<GenericResponse> RemoveFromAvailableAsync(Guid bookPublicId)
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
            catch (Exception ex)
            {
                // If something fails, return failure (logging handled elsewhere)
                return GenericResponse.FailureResponse("Při odstraňování došlo k chybě.");
            }
        }
    }
}
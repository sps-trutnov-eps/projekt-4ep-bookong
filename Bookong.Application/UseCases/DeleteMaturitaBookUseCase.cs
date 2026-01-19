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

        public async Task<GenericResponse> ExecuteAsync(Guid maturitaBookPublicId)
        {
            // Find the MaturitaBook directly
            var maturitaBook = await _unitOfWork.MaturitaBooks.GetByPublicIdAsync(maturitaBookPublicId);
            if (maturitaBook == null)
                return GenericResponse.FailureResponse("Kniha nebyla nalezena.");

            // Remove any user selections that reference physical books matching this maturita book
            try
            {
                var allBooks = await _unitOfWork.Books.GetAllAsync();
                
                // Find IDs of physical books that match the definition of the maturita book
                var matchingBookIds = allBooks
                    .Where(b => string.Equals(b.Name, maturitaBook.Name, StringComparison.OrdinalIgnoreCase)
                            && b.AuthorId == maturitaBook.AuthorId
                            && b.GenreId == maturitaBook.GenreId
                            && b.KindId == maturitaBook.KindId
                            && b.PeriodId == maturitaBook.PeriodId)
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
                _unitOfWork.MaturitaBooks.Delete(maturitaBook);
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

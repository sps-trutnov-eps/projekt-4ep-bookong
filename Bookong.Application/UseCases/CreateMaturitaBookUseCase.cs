using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Interfaces;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;

namespace Bookong.Application.UseCases
{
    public class CreateMaturitaBookUseCase : ICreateMaturitaBookUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateMaturitaBookUseCase(IUnitOfWork unitOfWork)
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

        public async Task<GenericResponse> ExecuteAsync(CreateMaturitaBookDto dto)
        {
            // Validate references
            var author = await _unitOfWork.Authors.GetByIdAsync(dto.AuthorId!.Value);
            if (author == null) return GenericResponse.FailureResponse("Vybraný autor neexistuje.");

            var genre = await _unitOfWork.Genres.GetByIdAsync(dto.GenreId!.Value);
            if (genre == null) return GenericResponse.FailureResponse("Vybraný žánr neexistuje.");

            var kind = await _unitOfWork.Kinds.GetByIdAsync(dto.KindId!.Value);
            if (kind == null) return GenericResponse.FailureResponse("Vybraný druh neexistuje.");

            var period = await _unitOfWork.Periods.GetByIdAsync(dto.PeriodId!.Value);
            if (period == null) return GenericResponse.FailureResponse("Vybrané období neexistuje.");

            // Check if already exists
            var existing = await _unitOfWork.MaturitaBooks.FindExistingAsync(
                dto.Name!,
                dto.AuthorId.Value,
                dto.GenreId.Value,
                dto.KindId.Value,
                dto.PeriodId.Value);

            if (existing != null)
                return GenericResponse.FailureResponse("Tato kniha již v seznamu existuje.");

            var maturitaBook = new MaturitaBook
            {
                Name = dto.Name!,
                AuthorId = dto.AuthorId.Value,
                GenreId = dto.GenreId.Value,
                KindId = dto.KindId.Value,
                PeriodId = dto.PeriodId.Value
            };

            _unitOfWork.MaturitaBooks.Add(maturitaBook);
            await _unitOfWork.CommitAsync();

            return GenericResponse.SuccessResponse("Kniha byla úspěšně přidána.");
        }
    }
}

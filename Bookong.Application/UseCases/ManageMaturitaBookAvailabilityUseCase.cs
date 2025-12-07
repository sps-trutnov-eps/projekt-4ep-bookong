using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Interfaces;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Bookong.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bookong.Application.UseCases
{
    public class ManageMaturitaBookAvailabilityUseCase : IManageMaturitaBookAvailabilityUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly BookongDbContext _dbContext;

        public ManageMaturitaBookAvailabilityUseCase(IUnitOfWork unitOfWork, BookongDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
        }

        public async Task<GenericResponse> AddToAvailableAsync(Guid bookPublicId)
        {
            var book = await _unitOfWork.Books.GetByPublicIdAsync(bookPublicId);
            if (book == null)
                return GenericResponse.FailureResponse("Kniha nebyla nalezena.");

            if (book.Author == null || book.Genre == null || book.Kind == null || book.Period == null)
                return GenericResponse.FailureResponse("Kniha nemá všechny požadované údaje.");

            // Check if already exists
            var existing = await _dbContext.MaturitaBooks
                .Include(mb => mb.Author)
                .Include(mb => mb.Genre)
                .Include(mb => mb.Kind)
                .Include(mb => mb.Period)
                .Where(mb => mb.Name == book.Name
                          && mb.Author.Id == book.Author.Id
                          && mb.Genre.Id == book.Genre.Id
                          && mb.Kind.Id == book.Kind.Id
                          && mb.Period.Id == book.Period.Id)
                .FirstOrDefaultAsync();

            if (existing != null)
                return GenericResponse.FailureResponse("Kniha již je v dostupných.");

            // Get fresh tracked entities
            var author = await _dbContext.Authors.FindAsync(book.Author.Id);
            var genre = await _dbContext.Genres.FindAsync(book.Genre.Id);
            var kind = await _dbContext.Kinds.FindAsync(book.Kind.Id);
            var period = await _dbContext.Periods.FindAsync(book.Period.Id);

            if (author == null || genre == null || kind == null || period == null)
                return GenericResponse.FailureResponse("Související entity nebyly nalezeny.");

            var maturitaBook = new MaturitaBook
            {
                Name = book.Name,
                Author = author,
                Genre = genre,
                Kind = kind,
                Period = period
            };

            _dbContext.MaturitaBooks.Add(maturitaBook);
            await _dbContext.SaveChangesAsync();

            return GenericResponse.SuccessResponse("Kniha byla přidána do dostupných.");
        }

        public async Task<GenericResponse> RemoveFromAvailableAsync(Guid bookPublicId)
        {
            var book = await _unitOfWork.Books.GetByPublicIdAsync(bookPublicId);
            if (book == null)
                return GenericResponse.FailureResponse("Kniha nebyla nalezena.");

            var existing = await _dbContext.MaturitaBooks
                .Include(mb => mb.Author)
                .Include(mb => mb.Genre)
                .Include(mb => mb.Kind)
                .Include(mb => mb.Period)
                .Where(mb => mb.Name == book.Name
                          && mb.Author!.Id == book.Author!.Id
                          && mb.Genre!.Id == book.Genre!.Id
                          && mb.Kind!.Id == book.Kind!.Id
                          && mb.Period!.Id == book.Period!.Id)
                .FirstOrDefaultAsync();

            if (existing == null)
                return GenericResponse.FailureResponse("Kniha není v dostupných.");

            _dbContext.MaturitaBooks.Remove(existing);
            await _dbContext.SaveChangesAsync();

            return GenericResponse.SuccessResponse("Kniha byla odebrána z dostupných.");
        }
    }
}
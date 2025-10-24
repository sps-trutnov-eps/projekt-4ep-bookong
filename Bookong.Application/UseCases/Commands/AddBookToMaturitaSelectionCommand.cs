using System;
using System.Linq;
using System.Threading.Tasks;
using Bookong.Application.UseCases.Commands.Interfaces;
using Bookong.Domain.Entities;
using Bookong.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bookong.Application.UseCases.Commands
{
    public class AddBookToMaturitaSelectionCommand : IAddBookToMaturitaSelectionCommand
    {
        private readonly BookongDbContext _dbContext;
        private readonly ILogger<AddBookToMaturitaSelectionCommand> _logger;

        public AddBookToMaturitaSelectionCommand(BookongDbContext dbContext, ILogger<AddBookToMaturitaSelectionCommand> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task ExecuteAsync(Guid bookPublicId)
        {
            try
            {
                // Get the first user from seeded data
                var user = await _dbContext.Users.FirstOrDefaultAsync()
                    ?? throw new InvalidOperationException("No users found in database. Please run seed data first.");

                // Get the book by its public ID
                var book = await _dbContext.Books.FirstOrDefaultAsync(b => b.PublicId == bookPublicId)
                    ?? throw new ArgumentException($"Book with PublicId {bookPublicId} not found", nameof(bookPublicId));

                // Check if the book is already in the selection
                var exists = await _dbContext.MaturitaBookSelections
                    .AnyAsync(m => m.BookId == book.Id && m.UserId == user.Id);

                if (!exists)
                {
                    _logger.LogInformation("Adding book {BookId} to maturita selection for user {UserId}", book.Id, user.Id);
                    
                    var selection = new MaturitaBookSelection
                    {
                        BookId = book.Id,
                        UserId = user.Id
                    };

                    _dbContext.MaturitaBookSelections.Add(selection);
                    await _dbContext.SaveChangesAsync();
                    
                    _logger.LogInformation("Book {BookId} successfully added to maturita selection for user {UserId}", book.Id, user.Id);
                }
                else
                {
                    _logger.LogInformation("Book {BookId} is already in maturita selection for user {UserId}", book.Id, user.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding book {BookPublicId} to maturita selection", bookPublicId);
                throw;
            }
        }
    }
}
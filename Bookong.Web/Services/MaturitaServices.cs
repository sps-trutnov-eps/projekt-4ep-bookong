using Bookong.Application.DTOs;
using Bookong.Domain.Entities;
using Bookong.Infrastructure.Data;

public class MaturitaService
{
    private readonly BookongDbContext _context;

    public MaturitaService(BookongDbContext context)
    {
        _context = context;
    }

    public async Task<List<BookListItemDto>> GetBooksAsync()
    {
        // join s knihami podle PublicId
        return await _context.MaturitaBooks
                             .Include(m => m.Book)
                             .Select(m => new BookListItemDto
                             {
                                 PublicId = m.Book.PublicId,
                                 Title = m.Book.Title,
                                 AuthorFullName = m.Book.AuthorFullName,
                                 Genre = m.Book.Genre,
                                 Kind = m.Book.Kind
                             }).ToListAsync();
    }

    public async Task AddBookAsync(BookListItemDto book)
    {
        if (!await _context.MaturitaBooks.AnyAsync(m => m.Book.PublicId == book.PublicId))
        {
            _context.MaturitaBooks.Add(new MaturitaBook { BookPublicId = book.PublicId });
            await _context.SaveChangesAsync();
        }
    }
}

public class MaturitaBook
{
    public int Id { get; set; }
    public Guid BookPublicId { get; set; }

    // navigační vlastnost
    public Book? Book { get; set; }
}

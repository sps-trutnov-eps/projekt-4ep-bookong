using Bookong.Application.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace Bookong.Web.Services
{
    public class MaturitaService
    {
        private readonly List<BookListItemDto> _maturitaBooks = new();

        public IReadOnlyList<BookListItemDto> GetBooks() => _maturitaBooks;

        public void AddBook(BookListItemDto book)
        {
            if (!_maturitaBooks.Any(b => b.PublicId == book.PublicId))
            {
                _maturitaBooks.Add(book);
            }
        }

        public void RemoveBook(BookListItemDto book)
        {
            _maturitaBooks.RemoveAll(b => b.PublicId == book.PublicId);
        }
    }
}

using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Bookong.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bookong.Infrastructure.Repositories
{
    public class ImportRepository(BookongDbContext context) : IImportRepository
    {
        private readonly BookongDbContext _context = context;

        public async Task<List<Author>> GetAllAuthorsAsync()
        {
            return await _context.Authors.AsNoTracking().ToListAsync();
        }

        public async Task<List<Publisher>> GetAllPublishersAsync()
        {
            return await _context.Publishers.AsNoTracking().ToListAsync();
        }

        public Author? FindAuthor(string firstName, string lastName, List<Author> authors)
        {
            if (string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName))
                return null;

            return authors.FirstOrDefault(a =>
                a.FirstName.Equals(firstName?.Trim() ?? "", StringComparison.OrdinalIgnoreCase) &&
                a.LastName.Equals(lastName?.Trim() ?? "", StringComparison.OrdinalIgnoreCase));
        }

        public Publisher? FindPublisher(string name, List<Publisher> publishers)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return publishers.FirstOrDefault(p => p.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public void CreateAuthorIfNotExists(string firstName, string lastName, List<Author> authors, ref bool needsCommit)
        {
            if (string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName))
                return;

            var trimmedFirstName = firstName?.Trim() ?? "";
            var trimmedLastName = lastName?.Trim() ?? "";

            var existingAuthor = authors.FirstOrDefault(a =>
                a.FirstName.Equals(trimmedFirstName, StringComparison.OrdinalIgnoreCase) &&
                a.LastName.Equals(trimmedLastName, StringComparison.OrdinalIgnoreCase));

            if (existingAuthor == null)
            {
                var author = new Author
                {
                    PublicId = Guid.NewGuid(),
                    FirstName = trimmedFirstName,
                    MiddleName = "",
                    LastName = trimmedLastName
                };
                _context.Authors.Add(author);
                authors.Add(author);
                needsCommit = true;
            }
        }

        public void CreatePublisherIfNotExists(string name, List<Publisher> publishers, ref bool needsCommit)
        {
            if (string.IsNullOrWhiteSpace(name))
                return;

            var trimmedName = name.Trim();
            var existingPublisher = publishers.FirstOrDefault(p => p.Name.Equals(trimmedName, StringComparison.OrdinalIgnoreCase));

            if (existingPublisher == null)
            {
                var publisher = new Publisher
                {
                    PublicId = Guid.NewGuid(),
                    Name = trimmedName
                };
                _context.Publishers.Add(publisher);
                publishers.Add(publisher);
                needsCommit = true;
            }
        }

        public void AddBook(Book book)
        {
            _context.Books.Add(book);
        }
    }
}

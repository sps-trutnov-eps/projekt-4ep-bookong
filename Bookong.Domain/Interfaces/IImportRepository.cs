using Bookong.Domain.Entities;

namespace Bookong.Domain.Interfaces
{
    public interface IImportRepository
    {
        Task<List<Author>> GetAllAuthorsAsync();
        Task<List<Publisher>> GetAllPublishersAsync();

        Author? FindAuthor(string firstName, string lastName, List<Author> authors);
        Publisher? FindPublisher(string name, List<Publisher> publishers);

        void CreateAuthorIfNotExists(string firstName, string lastName, List<Author> authors, ref bool needsCommit);
        void CreatePublisherIfNotExists(string name, List<Publisher> publishers, ref bool needsCommit);

        void AddBook(Book book);
    }
}

using Bookong.Domain.Entities;

namespace Bookong.Domain.Interfaces
{
    public interface IImportRepository
    {
        Task<List<Author>> GetAllAuthorsAsync();
        Task<List<Genre>> GetAllGenresAsync();
        Task<List<Kind>> GetAllKindsAsync();
        Task<List<Period>> GetAllPeriodsAsync();
        Task<List<Warehouse>> GetAllWarehousesAsync();

        Author? FindAuthor(string fullName, List<Author> authors);
        Genre? FindGenre(string name, List<Genre> genres);
        Kind? FindKind(string name, List<Kind> kinds);
        Period? FindPeriod(string name, List<Period> periods);
        Warehouse? FindWarehouse(string name, List<Warehouse> warehouses);

        void CreateAuthorIfNotExists(string fullName, List<Author> authors, ref bool needsCommit);
        void CreateGenreIfNotExists(string name, List<Genre> genres, ref bool needsCommit);
        void CreateKindIfNotExists(string name, List<Kind> kinds, ref bool needsCommit);
        void CreatePeriodIfNotExists(string name, List<Period> periods, ref bool needsCommit);
        void CreateWarehouseIfNotExists(string name, List<Warehouse> warehouses, ref bool needsCommit);

        void AddBook(Book book);
    }
}

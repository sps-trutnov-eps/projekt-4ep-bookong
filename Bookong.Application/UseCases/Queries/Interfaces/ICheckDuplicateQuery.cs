namespace Bookong.Application.UseCases.Queries.Interfaces
{
    public interface ICheckDuplicateQuery
    {
        Task<bool> AuthorExistsAsync(string firstName, string? middleName, string lastName);
        Task<bool> NameExistsAsync(string entityType, string name);
    }
}

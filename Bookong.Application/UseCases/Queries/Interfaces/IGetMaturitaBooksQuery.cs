namespace Bookong.Application.UseCases.Queries.Interfaces
{
    public interface IGetMaturitaBooksQuery
    {
        Task<IEnumerable<string>> ExecuteAsync();
    }
}
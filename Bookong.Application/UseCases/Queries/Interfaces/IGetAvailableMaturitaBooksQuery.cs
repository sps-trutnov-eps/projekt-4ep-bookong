namespace Bookong.Application.UseCases.Queries.Interfaces
{
    public interface IGetAvailableMaturitaBooksQuery
    {
        Task<IEnumerable<string>> ExecuteAsync();
    }
}
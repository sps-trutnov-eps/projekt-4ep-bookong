namespace Bookong.Application.Services.Interfaces
{
    public interface ISessionService
    {
        Task SetAsync(string key, string? value);
        Task<string?> GetAsync(string key);
        Task RemoveAsync(string key);
    }
}
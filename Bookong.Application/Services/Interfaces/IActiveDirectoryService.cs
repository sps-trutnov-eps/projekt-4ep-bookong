namespace Bookong.Application.Services.Interfaces
{
    public interface IActiveDirectoryService
    {
        Task<bool> ValidateCredetialsAsync(string username, string password);
    }
}
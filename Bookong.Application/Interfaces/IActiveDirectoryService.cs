namespace Bookong.Application.Interfaces
{
    public interface IActiveDirectoryService
    {
        Task<bool> ValidateCredetialsAsync(string username, string password);
        Task<string?> GetUserOranizationalUnit(string username);
    }
}
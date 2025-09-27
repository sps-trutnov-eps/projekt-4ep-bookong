namespace Bookong.Application.Services.Interface
{
    public interface IActiveDirectoryService
    {
        Task<bool> ValidateCredetialsAsync(string username, string password);
        Task<string?> GetUserOranizationalUnit(string username);
    }
}
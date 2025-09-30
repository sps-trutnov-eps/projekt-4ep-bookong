namespace Bookong.Application.Services.Interfaces
{
    public interface IActiveDirectoryService
    {
        Task<bool> ValidateCredetialsAsync(string username, string password);
        Task<string?> GetUserOranizationalUnitAsync(string username);
        Task<(string Name, string Email, Guid ObjectGuid)?> GetUserDetailsAsync(string username);
    }
}
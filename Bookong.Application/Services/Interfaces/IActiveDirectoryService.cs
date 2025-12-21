namespace Bookong.Application.Services.Interfaces
{
    public interface IActiveDirectoryService
    {
        Task<bool> ValidateCredentialsAsync(string username, string password);
    }
}

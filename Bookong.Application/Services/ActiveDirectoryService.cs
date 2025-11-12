using Bookong.Application.DTOs;
using Bookong.Application.Services.Interfaces;
using System.Net;





namespace Bookong.Application.Services
{
    public class ActiveDirectoryService : IActiveDirectoryService
    {
        // TODO: Implement real AD calls. Placeholders to avoid build errors.
        public Task<bool> ValidateCredetialsAsync(string username, string password)
        {
            return Task.FromResult(false);
        }

        public Task<string?> GetUserOranizationalUnitAsync(string username)
        {
            // Placeholder: no organizational unit known
            return Task.FromResult<string?>(null);
        }

        public Task<(string Name, string Email, Guid ObjectGuid)?> GetUserDetailsAsync(string username)
        {
            // Placeholder: no details found
            return Task.FromResult<(string Name, string Email, Guid ObjectGuid)?> (null);
        }
    }
}

namespace Bookong.Application.Services
{
    using Bookong.Application.Services.Interfaces;

    public class MockActiveDirectoryService : IActiveDirectoryService
    {
        private readonly Dictionary<string, string> _mockUsers = new()
        {
            { "admin", "admin123" },
            { "user1", "password1" },
            { "user2", "password2" },
            { "testuser", "test123" }
        };

        public Task<bool> ValidateCredetialsAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return Task.FromResult(false);
            }

            var isValid = _mockUsers.TryGetValue(username, out var storedPassword) 
                && storedPassword == password;

            return Task.FromResult(isValid);
        }

        public Task<string?> GetUserOranizationalUnitAsync(string username)
        {
            throw new NotImplementedException();
        }

        public Task<(string Name, string Email, Guid ObjectGuid)?> GetUserDetailsAsync(string username)
        {
            throw new NotImplementedException();
        }
    }
}

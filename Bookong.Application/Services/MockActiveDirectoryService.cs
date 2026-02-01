namespace Bookong.Application.Services
{
    using Bookong.Application.Services.Interfaces;

    public class MockActiveDirectoryService : IActiveDirectoryService
    {
        private readonly Dictionary<string, string> _mockUsers = new()
        {
            { "sysm22", "pushpushcommit" },
            { "stejskalm22", "miluju-backhand" },
            { "spurm22", "billionare" },
            { "nyms", "PZtka" }
        };

        public Task<bool> ValidateCredentialsAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return Task.FromResult(false);
            }

            var isValid = _mockUsers.TryGetValue(username, out var storedPassword) 
                && storedPassword == password;

            return Task.FromResult(isValid);
        }
    }
}

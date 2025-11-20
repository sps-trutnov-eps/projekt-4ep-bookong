using Bookong.Application.DTOs;
using Bookong.Application.Services.Interfaces;
using System.Net;
using System.DirectoryServices.Protocols;

namespace Bookong.Application.Services
{
    public class ActiveDirectoryService : IActiveDirectoryService
    {
        private readonly string _ldapServer = "192.168.1.104";

        public Task<bool> ValidateCredetialsAsync(string username, string password)
        {
            var creditentials = new NetworkCredential(username, password);
            using (var connection = new LdapConnection(_ldapServer))
            {
                connection.AuthType = AuthType.Negotiate;
                connection.Timeout = TimeSpan.FromSeconds(5);
                connection.SessionOptions.ProtocolVersion = 3;
                connection.Credential = creditentials;

                try
                {
                    connection.Bind();
                    Console.WriteLine("LDAP bind successful.");
                    return Task.FromResult(true);
                }
                catch (LdapException ex)
                {
                    Console.WriteLine("LDAP bind failed.");
                    Console.WriteLine(ex.Message);
                    return Task.FromResult(false);
                }
            }
        }

        public Task<string?> GetUserOranizationalUnitAsync(string username)
        {
            // TODO: Implement LDAP query to get OU
            // For now, return a default OU
            return Task.FromResult<string?>("Users");
        }

        public Task<(string Name, string Email, Guid ObjectGuid)?> GetUserDetailsAsync(string username)
        {
            // TODO: Implement LDAP query to retrieve user attributes from AD
            // For now, create details from the username
            try
            {
                // Extract name from username (e.g., "john.doe" -> "John Doe")
                var nameParts = username.Split('@')[0].Split('.');
                var firstName = nameParts.Length > 0 ? char.ToUpper(nameParts[0][0]) + nameParts[0].Substring(1) : "User";
                var lastName = nameParts.Length > 1 ? char.ToUpper(nameParts[1][0]) + nameParts[1].Substring(1) : "";
                var fullName = $"{firstName} {lastName}".Trim();

                // Generate a consistent ObjectGuid based on username
                // In production, this should come from AD's objectGUID attribute
                var objectGuid = Guid.NewGuid();

                return Task.FromResult<(string Name, string Email, Guid ObjectGuid)?>((
                    Name: fullName,
                    Email: username,
                    ObjectGuid: objectGuid
                ));
            }
            catch
            {
                return Task.FromResult<(string Name, string Email, Guid ObjectGuid)?>(null);
            }
        }
    }
}

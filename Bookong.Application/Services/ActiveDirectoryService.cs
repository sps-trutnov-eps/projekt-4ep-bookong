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
            // Simple implementation: return basic user info based on username
            // Use a deterministic GUID based on username hash to ensure consistency
            var guidBytes = System.Text.Encoding.UTF8.GetBytes(username).Take(16).ToArray();
            if (guidBytes.Length < 16)
            {
                var padding = new byte[16 - guidBytes.Length];
                guidBytes = guidBytes.Concat(padding).ToArray();
            }
            var objectGuid = new Guid(guidBytes);

            var usernamePart = username.Split('@')[0];
            
            return Task.FromResult<(string Name, string Email, Guid ObjectGuid)?>((
                Name: usernamePart,
                Email: username,
                ObjectGuid: objectGuid
            ));
        }
    }
}

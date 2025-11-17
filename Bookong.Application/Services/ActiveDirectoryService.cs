using Bookong.Application.DTOs;
using Bookong.Application.Services.Interfaces;
using System.Net;
using System.DirectoryServices.Protocols;





namespace Bookong.Application.Services
{
    public class ActiveDirectoryService : IActiveDirectoryService
    {
        // TODO: Implement real AD calls. Placeholders to avoid build errors.
        public Task<bool> ValidateCredetialsAsync(string username, string password)
        {
            var creditentials = new NetworkCredential(username, password);
            var Ldapserver = "192.168.1.104";
            using (var connection = new LdapConnection(Ldapserver))
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

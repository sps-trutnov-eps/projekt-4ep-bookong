using Bookong.Application.DTOs;
using Bookong.Application.Services.Interfaces;
using System.Net;
using System.DirectoryServices.Protocols;

namespace Bookong.Application.Services
{
    public class ActiveDirectoryService : IActiveDirectoryService
    {
        private readonly string _ldapServer = "skola.local";

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
    }
}

using Bookong.Application.DTOs;
using Bookong.Application.Services.Interfaces;
using System.Net;
using System.DirectoryServices.Protocols;

namespace Bookong.Application.Services
{
    public class ActiveDirectoryService : IActiveDirectoryService
    {
        private readonly string _ldapServer = "gateway.spstrutnov.cz:389";

        public Task<bool> ValidateCredentialsAsync(string username, string password)
        {
            string domain = "skola.local";
            if (!username.Contains("@"))
            {
                username = $"{username}@{domain}";
            }
            var credentials = new NetworkCredential(username, password);
            using (var connection = new LdapConnection(_ldapServer))
            {
                connection.AuthType = AuthType.Basic;
                connection.Timeout = TimeSpan.FromSeconds(5);
                connection.SessionOptions.ProtocolVersion = 3;
                connection.SessionOptions.SecureSocketLayer = false;
                connection.Credential = credentials;

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

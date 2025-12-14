using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Bookong.Application.Services.Interfaces;

namespace Bookong.Web.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly CookieAuthenticationStateProvider _stateProvider;

        public CurrentUserService(CookieAuthenticationStateProvider stateProvider)
        {
            _stateProvider = stateProvider;
        }

        public async Task<Guid?> GetCurrentUserPublicIdAsync()
        {
            var authState = await _stateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user?.Identity?.IsAuthenticated != true)
            {
                return null;
            }

            var publicIdValue = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(publicIdValue, out var publicId) ? publicId : null;
        }
    }
}

using Bookong.Application.Services.Interfaces;

namespace Bookong.Web.Services
{
    public class SessionService(IHttpContextAccessor ctx) : ISessionService
    {
        private readonly IHttpContextAccessor _ctx = ctx;

        public Task SetAsync(string key, string? value)
        {
            _ctx.HttpContext!.Session.SetString(key, value ?? string.Empty);
            return Task.CompletedTask;
        }

        public Task<string?> GetAsync(string key)
        {
            var v = _ctx.HttpContext!.Session.GetString(key);
            return Task.FromResult<string?>(string.IsNullOrEmpty(v) ? null : v);
        }

        public Task RemoveAsync(string key)
        {
            _ctx.HttpContext!.Session.Remove(key);
            return Task.CompletedTask;
        }

        public Task ClearAsync()
        {
            _ctx.HttpContext!.Session.Clear();
            return Task.CompletedTask;
        }
    }
}
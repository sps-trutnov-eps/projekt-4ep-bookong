using Bookong.Application.Services.Interfaces;
using Bookong.Domain.Entities;
using Bookong.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bookong.Web.Services
{
    public class TestUserMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;
        private const string SessionKey = "CurrentUserPublicId";

        public async Task InvokeAsync(HttpContext context, BookongDbContext db, ISessionService session)
        {
            if (context.Session is not null)
            {
                var path = context.Request.Path.Value?.ToLowerInvariant();
                var isLoginPage = path != null && path.Contains("/login");

                var existing = await session.GetAsync(SessionKey);
                if (string.IsNullOrEmpty(existing) && !isLoginPage)
                {
                    var user = await db.Users.OrderBy(u => u.Id).FirstOrDefaultAsync();
                    if (user == null)
                    {
                        user = new User
                        {
                            SamAccountName = "test.user",
                            ObjectGuid = Guid.NewGuid(),
                            Name = "Test User",
                            Email = "test.user@example.local",
                            OrganizationalUnit = "TestOU"
                        };
                        db.Users.Add(user);
                        await db.SaveChangesAsync();
                    }

                    await session.SetAsync(SessionKey, user.PublicId.ToString());
                }
            }

            await _next(context);
        }
    }
}
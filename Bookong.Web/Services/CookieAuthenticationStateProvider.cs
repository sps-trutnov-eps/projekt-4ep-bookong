using System.Security.Claims;
using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;

namespace Bookong.Web.Services
{
    public class CookieAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoginUserUseCase _loginUserUseCase;

        public CookieAuthenticationStateProvider(
            IHttpContextAccessor httpContextAccessor,
            ILoginUserUseCase loginUserUseCase)
        {
            _httpContextAccessor = httpContextAccessor;
            _loginUserUseCase = loginUserUseCase;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            
            if (httpContext == null)
            {
                return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
            }

            var principal = httpContext.User ?? new ClaimsPrincipal(new ClaimsIdentity());

            return Task.FromResult(new AuthenticationState(principal));
        }

        public async Task<LoginResult> SignInAsync(LoginUserRequest request)
        {
            var response = await _loginUserUseCase.ExecuteAsync(request);

            if (response.IsFirstTimeUser)
            {
                return new LoginResult
                {
                    Success = false,
                    IsFirstTimeUser = true,
                    Message = response.Message,
                    Username = response.Username ?? request.Email
                };
            }

            if (response.UserId == Guid.Empty)
            {
                return new LoginResult
                {
                    Success = false,
                    IsFirstTimeUser = false,
                    Message = response.Message,
                    Username = response.Username ?? request.Email
                };
            }

            var user = new UserSessionInfo
            {
                UserId = response.UserId,
                Username = response.Username ?? request.Email,
                Name = response.Name ?? response.Username ?? request.Email,
                Email = response.Email ?? $"{response.Username}@spstrutnov.cz",
                OrganizationalUnit = response.OrganizationalUnit ?? "Default"
            };

            await SignInKnownUserAsync(user);

            return new LoginResult
            {
                Success = true,
                IsFirstTimeUser = false,
                Message = response.Message,
                Username = user.Username,
                User = user
            };
        }

        public async Task SignInKnownUserAsync(UserSessionInfo user)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                throw new InvalidOperationException("HttpContext is not available.");
            }

            var principal = BuildPrincipal(user);

            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                });

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task LogoutAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        private static ClaimsPrincipal BuildPrincipal(UserSessionInfo user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Email, user.Email),
                new("organizational_unit", user.OrganizationalUnit),
                new("display_name", user.Name)
            };

            var role = string.Equals(user.OrganizationalUnit, "Admin", StringComparison.OrdinalIgnoreCase)
                ? "admin"
                : "default";

            claims.Add(new Claim(ClaimTypes.Role, role));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            return new ClaimsPrincipal(identity);
        }
    }

    public class LoginResult
    {
        public bool Success { get; init; }
        public bool IsFirstTimeUser { get; init; }
        public string? Message { get; init; }
        public string? Username { get; init; }
        public UserSessionInfo? User { get; init; }
    }

    public class UserSessionInfo
    {
        public Guid UserId { get; init; }
        public required string Username { get; init; }
        public required string Name { get; init; }
        public required string Email { get; init; }
        public required string OrganizationalUnit { get; init; }
    }
}

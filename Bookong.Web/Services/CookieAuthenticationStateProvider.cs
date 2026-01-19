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
            ArgumentNullException.ThrowIfNull(request);

            var response = await _loginUserUseCase.ExecuteAsync(request);

            if (response.IsFirstTimeUser || response.UserId == Guid.Empty)
            {
                return CreateLoginResult(response, request);
            }

            var user = CreateUserSession(response, request);
            await SignInKnownUserAsync(user);

            return CreateLoginResult(response, request, user);
        }

        public async Task SignInKnownUserAsync(UserSessionInfo user)
        {
            ArgumentNullException.ThrowIfNull(user);

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

            var isAdmin = string.Equals(user.OrganizationalUnit, "Admin", StringComparison.OrdinalIgnoreCase);
            var role = isAdmin ? "Admin" : "Default";

            claims.Add(new Claim(ClaimTypes.Role, role));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            return new ClaimsPrincipal(identity);
        }

        private static LoginResult CreateLoginResult(LoginUserResponse response, LoginUserRequest request, UserSessionInfo? user = null)
        {
            return new LoginResult
            {
                Success = user is not null,
                IsFirstTimeUser = response.IsFirstTimeUser,
                Message = response.Message,
                Username = response.Username ?? request.Email,
                User = user
            };
        }

        private static UserSessionInfo CreateUserSession(LoginUserResponse response, LoginUserRequest request)
        {
            var username = response.Username ?? request.Email;
            var fallbackEmail = response.Username is not null
                ? $"{response.Username}@spstrutnov.cz"
                : request.Email;

            return new UserSessionInfo
            {
                UserId = response.UserId,
                Username = username,
                Name = response.Name ?? username,
                Email = response.Email ?? fallbackEmail,
                OrganizationalUnit = response.OrganizationalUnit ?? "Default"
            };
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

using System;
using System.Threading.Tasks;
using Bookong.Application.DTOs;
using Microsoft.JSInterop;

namespace Bookong.Web.Services;

public class AuthJsInterop
{
    private readonly IJSRuntime _jsRuntime;

    public AuthJsInterop(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
    }

    public ValueTask<LoginResult?> LoginAsync(LoginUserRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return _jsRuntime.InvokeAsync<LoginResult?>("authApi.login", request);
    }

    public ValueTask<CompleteUserRegistrationResponse?> CompleteRegistrationAsync(CompleteUserRegistrationRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return _jsRuntime.InvokeAsync<CompleteUserRegistrationResponse?>("authApi.register", request);
    }

    public ValueTask LogoutAsync()
    {
        return _jsRuntime.InvokeVoidAsync("authApi.logout");
    }
}

using System.Security.Claims;
using KERP.Client.DTOs;
using Microsoft.AspNetCore.Components.Authorization;

namespace KERP.Client.Services.Auth;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private const string LocalStorageKey = "currentUser";

    private readonly LocalStorageService _localStorageService;

    public CustomAuthenticationStateProvider(LocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var currentUser = await GetCurrentUserAsync();

        if(currentUser == null)
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        Claim[] claims = [
            new Claim(ClaimTypes.NameIdentifier, currentUser.Id),
            new Claim(ClaimTypes.Name, currentUser.Username),
            new Claim(ClaimTypes.Email, currentUser.Username)
        ];

        var authenticationState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType: nameof(CustomAuthenticationStateProvider))));

        return authenticationState;
    }

    public async Task SetCurrentUserAsync(User? currentUser)
    { 
        await _localStorageService.SetItem(LocalStorageKey, currentUser);

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public Task<User?> GetCurrentUserAsync() => _localStorageService.GetItemAsync<User>(LocalStorageKey);
}
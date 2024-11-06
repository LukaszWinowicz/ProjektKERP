using KERP.Client.Settings;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

namespace KERP.Client.Services.Auth;

public class IdentityProviderService(
    IOptions<ApiSettings> apiSettings,
    IHttpClientFactory httpClientFactory, 
    NavigationManager navigationManager, 
    LocalStorageService browserStorage)
{
    public async Task AuthenticateWithGoogle()
    {
        var httpClient = httpClientFactory.CreateClient();

        // Call our backend API to retrieve a URL for Google authentication.
        // Preserve the current URI in the state, so we can return the user back
        // to the same page after successful authentication
        var httpResponseMessage = await httpClient.GetAsync($"api/auth/redirect/google?state={navigationManager.Uri}");

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            // Navigate the user to the Google authentication page
            var googleAuthenticationUrl = await httpResponseMessage.Content.ReadAsStringAsync();
            navigationManager.NavigateTo(googleAuthenticationUrl);
        }
    }
}
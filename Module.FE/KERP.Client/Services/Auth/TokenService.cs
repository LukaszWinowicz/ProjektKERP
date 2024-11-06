using KERP.Client.Settings;
using Microsoft.Extensions.Options;

namespace KERP.Client.Services.Auth;

public class TokenService(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiSettings)
{
    public async Task<string?> RefreshAccessToken(string oldToken, CancellationToken cancellationToken = default)
    {
        using var client = httpClientFactory.CreateClient();

        var tokenRefreshResponse = await client.PostAsJsonAsync($"api/token/refresh", oldToken, cancellationToken);
        if (tokenRefreshResponse.IsSuccessStatusCode)
        {
            var newAccessToken = await tokenRefreshResponse.Content.ReadAsStringAsync(cancellationToken);
            return newAccessToken;
        }
        else
        {
            return null;
        }
    }
}

using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace KERP.Client.Services;

public class CookieDelegatingHandler : HttpClientHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        return await base.SendAsync(request, cancellationToken);
    }
}
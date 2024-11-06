using KERP.Client.Components;
using KERP.Client.Services;
using KERP.Client.Services.Auth;
using KERP.Client.Settings;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

// Add services to the container.
builder.Services.AddRazorComponents();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddFluentUIComponents();

builder.Services.AddAuthorization();
builder.Services.AddSingleton<MassUpdatePurchaseApi>();
builder.Services.AddScoped<IdentityProviderService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddTransient<CookieDelegatingHandler>();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<CustomAuthenticationStateProvider>());
builder.Services.AddScoped<LocalStorageService>();

builder.Services.Configure<CookiePolicyOptions>(options => 
{ 
    options.MinimumSameSitePolicy = SameSiteMode.Lax; 
}); 

builder.Services.AddHttpClient("", (serviceProvider, httpClient) =>
    {
        var apiSettings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>();
        httpClient.BaseAddress = new Uri(apiSettings.Value.BaseUrl);
    })
    .ConfigurePrimaryHttpMessageHandler<CookieDelegatingHandler>();

var app = builder.Build();

await app.RunAsync();

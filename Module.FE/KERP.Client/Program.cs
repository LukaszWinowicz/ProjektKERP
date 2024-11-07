using KERP.Client.Components;
using KERP.Client.Services;
using KERP.Client.Services.Auth;
using KERP.Client.Settings;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddFluentUIComponents();

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

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
app.UseCookiePolicy();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

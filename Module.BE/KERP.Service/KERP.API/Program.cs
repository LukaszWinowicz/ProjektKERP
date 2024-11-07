using Azure.Identity;
using KERP.API.Components;
using KERP.API.Options;
using KERP.API.Services;
using KERP.Core.Interfaces.Repositories;
using KERP.Core.Interfaces.Services;
using KERP.Core.Services;
using KERP.Infrastructure.Database;
using KERP.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddFluentUIComponents();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IMassUpdatePurchaseRepository, MassUpdatePurchaseRepository>();
builder.Services.AddScoped<IMassUpdatePurchaseService, MassUpdatePurchaseService>();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection("Authentication"));
builder.Services.Configure<AuthorizationOptions>(builder.Configuration.GetSection("Authorization"));

builder.Services.AddDbContext<ServiceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
    })
    .AddEntityFrameworkStores<ServiceDbContext>();

builder.Services.AddAuthentication()
    .AddCookie();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseWebAssemblyDebugging();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorComponents<App>() // Map Razor components
    .AddInteractiveServerRenderMode();

app.Run();

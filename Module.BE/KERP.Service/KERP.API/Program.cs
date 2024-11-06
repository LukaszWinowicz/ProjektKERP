using Azure.Identity;
using KERP.API.Options;
using KERP.API.Services;
using KERP.Core.Interfaces.Repositories;
using KERP.Core.Interfaces.Services;
using KERP.Core.Services;
using KERP.Infrastructure.Database;
using KERP.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
    

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(b => b
        .WithOrigins("https://127.0.0.1:7000")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
});

builder.Services.AddAuthentication()
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.None;

        options.Events.OnRedirectToAccessDenied = (context) => throw new AuthenticationFailedException("");
        options.Events.OnRedirectToLogin = (context) => throw new AuthenticationFailedException("");
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseWebAssemblyDebugging();
}

app.UseBlazorFrameworkFiles();
app.MapFallbackToFile("index.html");

app.UseHttpsRedirection();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

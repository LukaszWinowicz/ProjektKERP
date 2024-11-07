using System.Security.Claims;
using KERP.API.Models.Contracts;
using KERP.API.Options;
using KERP.API.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace KERP.API.Controllers;

[ApiController]
[Route("signin-google")]
public class AuthController(
    IOptions<AuthOptions> authenticationOptions,
    IAuthService authService,
    SignInManager<IdentityUser> signInManager,
    UserManager<IdentityUser> userManager) : ControllerBase
{
    [HttpGet]
    [Route("redirect/google")]
    [AllowAnonymous]
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    public IActionResult GetGoogleRedirect([FromQuery] string state)
    {
        var config = authenticationOptions.Value.GoogleOptions!;

        var googleAuthUrl =
            $"{config.AuthenticationEndpoint}?" +
            $"response_type={config.ResponseType}&" +
            $"client_id={config.ClientId}&" +
            $"redirect_uri={config.RedirectUri}&" +
            $"scope={config.Scope}&" +
            $"state={state}&" +
            $"nonce={Guid.NewGuid()}";

        return Redirect(googleAuthUrl);
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> SigninWithGoogle(string code)
    {
        var authenticationResult = await authService.AuthenticateWithGoogleAsync(code!);
        if (authenticationResult.IsFailure)
        {
            return Unauthorized();
        }

        var user = await userManager.FindByEmailAsync(authenticationResult.Payload.Email);
        if (user == null)
        {
            user = new IdentityUser
            {
                UserName = authenticationResult.Payload.Email,
                Email = authenticationResult.Payload.Email
            };

            await userManager.CreateAsync(user);
        }

        await userManager.AddClaimAsync(user, new Claim("GivenName", authenticationResult.Payload.Name));
        var claims = await userManager.GetClaimsAsync(user);
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        await signInManager.SignInAsync(user, isPersistent: true);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal,
            new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTime.UtcNow.AddDays(30) });

        return Redirect("/");
    }
}
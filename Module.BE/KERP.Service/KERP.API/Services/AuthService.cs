using Google.Apis.Auth;
using KERP.API.Models.Contracts;
using KERP.API.Models.Entities;
using KERP.API.Options;
using KERP.API.Utils;
using Microsoft.Extensions.Options;

namespace KERP.API.Services;

public interface IAuthService
{
    Task<Result<AuthResponse>> AuthenticateWithGoogleAsync(string authorizationCode, CancellationToken cancellationToken = default);
}

internal sealed class AuthService(
    IHttpClientFactory httpClientFactory,
    IOptions<AuthOptions> options,
    IUserService userService,
    ITokenService tokenService) : IAuthService
{
    public async Task<Result<AuthResponse>> AuthenticateWithGoogleAsync(string authorizationCode, CancellationToken cancellationToken)
    {
        var config = options.Value.GoogleOptions!;

        var idTokenRequestContent = new FormUrlEncodedContent
        ([
            new KeyValuePair<string, string>("code", authorizationCode),
            new KeyValuePair<string, string>("client_id", config.ClientId),
            new KeyValuePair<string, string>("client_secret", config.ClientSecret),
            new KeyValuePair<string, string>("redirect_uri", config.RedirectUri),
            new KeyValuePair<string, string>("grant_type", "authorization_code")
        ]);

        // Exchange the Google authorization code for access token
        var authorizationCodeExchangeRequest = await httpClientFactory.CreateClient().PostAsync(
            config.AuthorizationCodeEndpoint, idTokenRequestContent, cancellationToken);

        if (!authorizationCodeExchangeRequest.IsSuccessStatusCode)
        {
            var responseMessage = await authorizationCodeExchangeRequest.Content.ReadAsStringAsync(cancellationToken);
            return Result.Failure<AuthResponse>(errorMessage: $"Authorization code exchange failed. {responseMessage}");
        }

        var idTokenContent = await authorizationCodeExchangeRequest.Content.ReadFromJsonAsync<GoogleTokenResponse>(cancellationToken);
        if (idTokenContent?.id_token is null)
        {
            return Result.Failure<AuthResponse>(errorMessage: "id_token not found in the response from the identity provider");
        }

        try
        {
            var validatedUser = await GoogleJsonWebSignature.ValidateAsync
            (
                validationSettings: new() { Audience = [config.ClientId] },
                jwt: idTokenContent?.id_token
            );

            return Result.Success(new AuthResponse
            (
                ProfilePicture: validatedUser.Picture,
                ExternalId: validatedUser.Subject,
                Username: validatedUser.Name,
                Email: validatedUser.Email,
                Name: validatedUser.GivenName
            ));
        }
        catch (InvalidJwtException e)
        {
            return Result.Failure<AuthResponse>(errorMessage: $"The id_token did not pass validation. {e.Message}");
        }
        catch (Exception e)
        {
            return Result.Failure<AuthResponse>(errorMessage: $"Encountered an error during id_token validation. {e.Message}");
        }
    }

    private bool UserInfoChanged(User currentInfo, AuthResponse newInfo) => 
        currentInfo.Username != newInfo.Username || 
        currentInfo.Email != newInfo.Email ||
        currentInfo.ProfilePicture != newInfo.ProfilePicture;
}

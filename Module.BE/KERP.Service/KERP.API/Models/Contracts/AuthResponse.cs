namespace KERP.API.Models.Contracts;

public sealed record AuthResponse(
    string ExternalId,
    string Username,
    string Name,
    string Email,
    string? ProfilePicture
);

namespace KERP.API.Models.Contracts;

public sealed record RefreshToken(
    string Value,
    DateTime ExpiresUtc
);

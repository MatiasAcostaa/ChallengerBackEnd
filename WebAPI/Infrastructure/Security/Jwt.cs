namespace WebAPI.Infrastructure.Security;

public record Jwt
{
    public string Secret { get; init; } = null!;
};
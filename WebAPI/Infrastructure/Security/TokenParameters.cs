using WebAPI.Infrastructure.Security.Interfaces;

namespace WebAPI.Infrastructure.Security;

public record TokenParameters : ITokenParameters
{
    public string Id { get; init; } = null!;
    public string UserName { get; init; } = null!;
    public string PasswordHash { get; init; } = null!;
};
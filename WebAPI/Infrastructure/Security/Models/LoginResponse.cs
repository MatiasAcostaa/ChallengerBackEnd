namespace WebAPI.Infrastructure.Security.Models;

public class LoginResponse
{
    public string Token { get; init; } = null!;
    public bool Login { get; init; }
    public IList<string> Errors { get; init; } = null!;
}
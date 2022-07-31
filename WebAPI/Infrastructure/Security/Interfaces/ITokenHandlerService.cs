namespace WebAPI.Infrastructure.Security.Interfaces;

public interface ITokenHandlerService
{
    string GenerateJwtToken(ITokenParameters parameters);
}
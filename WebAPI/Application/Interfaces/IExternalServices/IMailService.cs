namespace WebAPI.Application.Interfaces.IExternalServices;

public interface IMailService
{
    Task<object> SendMail(string to);
}
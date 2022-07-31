using System.Text;
using FluentEmail.Core;
using FluentEmail.Razor;
using WebAPI.Application.Interfaces.IExternalServices;

namespace WebAPI.Infrastructure.Services.Mail;

public class MailService : IMailService
{
    private readonly IFluentEmail _email;

    public MailService(IFluentEmail email)
    {
        _email = email;
    }
    
    public async Task<object> SendMail(string to)
    {
        StringBuilder template = new();
        template.AppendLine("Estimado @Model.FirstName,");
        template.AppendLine("<p>Gracias por ingresar</p>");
        template.AppendLine("- Matias Acosta");

        //Email.DefaultSender = sender;
        Email.DefaultRenderer = new RazorRenderer();
        
        var email = await _email
            .To(to)
            .Subject("¡Gracias!")
            .UsingTemplate(template.ToString(), new { FirstName = "Matias Acosta" })
            .SendAsync();

        return email;
    }
}
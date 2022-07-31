using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Application.Interfaces.IExternalServices;
using WebAPI.Application.Interfaces.IExternalServices.IRepositories;
using WebAPI.Infrastructure.Persistence;
using WebAPI.Infrastructure.Persistence.Repositories;
using WebAPI.Infrastructure.Security;
using WebAPI.Infrastructure.Security.Interfaces;
using WebAPI.Infrastructure.Services.Local;
using WebAPI.Infrastructure.Services.Mail;

namespace WebAPI.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(ops => 
            ops.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IFileStorage, LocalFileStorage>();
        
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        
        services.AddScoped<ICharacterRepository, CharacterRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<IMovieRepository, MovieRepository>();

        return services;
    }

    public static IServiceCollection Security(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<Jwt>(configuration.GetSection("Jwt"));

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt =>
            {
                var key = Encoding.ASCII.GetBytes(configuration["Jwt:Secret"]);

                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = true
                };
            });

        services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddScoped<ITokenHandlerService, TokenHandlerService>();
        
        return services;
    }
    
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddFluentEmail("challengeback2022@outlook.com")
            .AddRazorRenderer()
            .AddSmtpSender(new SmtpClient("smtp.outlook.com")
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(userName: "challengeback2022@outlook.com", password: "Challenge2022"),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Port = 587
            });
        
        services.AddScoped<IMailService, MailService>();
    
        return services;
    }
}
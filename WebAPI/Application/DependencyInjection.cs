using System.Reflection;
using WebAPI.Application.Interfaces.IExternalServices.IServices;
using WebAPI.Application.Services;

namespace WebAPI.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        services.AddScoped<IGenresService, GenresService>();
        services.AddScoped<IMoviesService, MoviesService>();
        services.AddScoped<ICharactersService, CharactersService>();

        return services;
    }
}
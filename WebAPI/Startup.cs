using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using WebAPI.Application;
using WebAPI.Extensions;
using WebAPI.Infrastructure;

namespace WebAPI;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    
    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDataAccess(Configuration);
        services.Security(Configuration);
        services.ConfigureServices();
        services.AddApplication();
        
        services.AddAutoMapper(typeof(Startup));

        services.AddHttpContextAccessor();
        
        services.AddControllers()
            .AddFluentValidation(f => f.RegisterValidatorsFromAssemblyContaining<Startup>());
        
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI", Version = "v1" });
            
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                In = ParameterLocation.Header,
                Description = "Escribe en el cuadro de texto: Bearer {tu JWT Token}"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });
        
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));
        }

        app.UseHttpsRedirection();
        
        app.UseStaticFiles();
        
        app.UseRouting();

        app.UseAuthorization();

        app.UseErrorHandlingMiddleware();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
    
    
    
}
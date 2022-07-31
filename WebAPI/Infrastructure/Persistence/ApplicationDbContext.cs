using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAPI.Domain.Entities;

namespace WebAPI.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options) { }
    
    public DbSet<Genre> Genres => Set<Genre>();

    public DbSet<Movie> Movies => Set<Movie>();
    
    public DbSet<Character> Characters => Set<Character>();

    public DbSet<MovieGenre> MovieGenres => Set<MovieGenre>();

    public DbSet<MovieCharacter> MovieCharacters => Set<MovieCharacter>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(modelBuilder);
        
    }
    
}
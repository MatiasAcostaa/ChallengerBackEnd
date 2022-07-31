using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebAPI.Application.Interfaces.IExternalServices.IRepositories;
using WebAPI.Domain.Entities;

namespace WebAPI.Infrastructure.Persistence.Repositories;

public class MovieRepository : Repository<Movie>, IMovieRepository
{
    public MovieRepository(ApplicationDbContext context) : base(context) { }
    
    public override IQueryable<Movie> GetQuery() =>
        Context.Movies
            .AsNoTracking()
            .Include(m => m.MovieGenres)
            .ThenInclude(mg => mg.Genre);

    public override async Task<Movie?> GetByIdAsync(Expression<Func<Movie, bool>> predicate) =>
        await Context.Movies
            .AsNoTracking()
            .Include(m => m.MovieGenres)
            .ThenInclude(mg => mg.Genre)
            .Include(a => a.MovieCharacters)
            .ThenInclude(ma => ma.Character)
            .FirstOrDefaultAsync(predicate);
}
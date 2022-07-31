using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebAPI.Application.Interfaces.IExternalServices.IRepositories;
using WebAPI.Domain.Entities;

namespace WebAPI.Infrastructure.Persistence.Repositories;

public class CharacterRepository : Repository<Character>, ICharacterRepository
{
    public CharacterRepository(ApplicationDbContext context) : base(context) { }
    
    public override IQueryable<Character> GetQuery() =>
        Context.Characters
            .AsNoTracking()
            .Include(a => a.MovieCharacters)
            .ThenInclude(ma => ma.Movie);

    public override async Task<Character?> GetByIdAsync(Expression<Func<Character, bool>> predicate) =>
        await Context.Characters
            .AsNoTracking()
            .Include(a => a.MovieCharacters)
            .ThenInclude(ma => ma.Movie)
            .FirstOrDefaultAsync(predicate);
}
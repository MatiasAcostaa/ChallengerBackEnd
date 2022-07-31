using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebAPI.Application.Interfaces.IExternalServices.IRepositories;

namespace WebAPI.Infrastructure.Persistence.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly ApplicationDbContext Context;


    public Repository(ApplicationDbContext context)
    {
        Context = context;
    }
    
    public virtual IQueryable<TEntity> GetQuery() => Context.Set<TEntity>().AsTracking().AsQueryable();

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync() =>
        await Context.Set<TEntity>().AsNoTracking().ToListAsync();

    public virtual async Task<TEntity?> GetByIdAsync(Expression<Func<TEntity, bool>> predicate) =>
        await Context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(predicate);

    public virtual async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate) =>
        await Context.Set<TEntity>().FirstOrDefaultAsync(predicate);

    public void Create(TEntity entity) => Context.Set<TEntity>().Add(entity);

    public void Update(TEntity entity) => Context.Set<TEntity>().Update(entity);

    public void Delete(TEntity entity) => Context.Set<TEntity>().Remove(entity);

    public async Task SaveChangesAsync() => await Context.SaveChangesAsync();
}
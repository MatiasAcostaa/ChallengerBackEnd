using System.Linq.Expressions;

namespace WebAPI.Application.Interfaces.IExternalServices.IRepositories;

public interface IRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> GetQuery();
    
    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<TEntity?> GetByIdAsync(Expression<Func<TEntity, bool>> predicate);

    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);

    void Create(TEntity entity);
    
    void Update(TEntity entity);
    
    void Delete(TEntity entity);
    
    Task SaveChangesAsync();
}
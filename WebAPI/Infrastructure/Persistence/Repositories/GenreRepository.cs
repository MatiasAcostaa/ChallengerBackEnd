using WebAPI.Application.Interfaces.IExternalServices.IRepositories;
using WebAPI.Domain.Entities;

namespace WebAPI.Infrastructure.Persistence.Repositories;

public class GenreRepository : Repository<Genre>, IGenreRepository
{
    public GenreRepository(ApplicationDbContext context) : base(context) { }
}
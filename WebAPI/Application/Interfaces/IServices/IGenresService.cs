using WebAPI.Application.DTOs.Genres;

namespace WebAPI.Application.Interfaces.IExternalServices.IServices;

public interface IGenresService
{
    Task<IEnumerable<GenreDto>> GetGenres();

    Task<int> CreateGenre(CreateGenreDto genreDto);

    Task UpdateGenre(int id, CreateGenreDto genreDto);

    Task DeleteGenre(int id);
}
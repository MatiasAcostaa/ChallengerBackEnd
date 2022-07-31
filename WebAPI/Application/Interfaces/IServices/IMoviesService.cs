using WebAPI.Application.DTOs.Movies;

namespace WebAPI.Application.Interfaces.IExternalServices.IServices;

public interface IMoviesService
{
    Task<IEnumerable<MoviesDto>> GetMovies(MoviesRequestDto request);

    Task<MovieDto> GetMovieDetails(int id);

    Task<int> CreateMovie(CreateMovieDto movieDto);

    Task UpdateMovie(int id, UpdateMovieDto movieDto);

    Task DeleteMovie(int id);
}
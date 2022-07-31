using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WebAPI.Application.DTOs.Movies;
using WebAPI.Application.Interfaces.IExternalServices;
using WebAPI.Application.Interfaces.IExternalServices.IRepositories;
using WebAPI.Application.Interfaces.IExternalServices.IServices;
using WebAPI.Domain.Entities;
using WebAPI.Domain.Enums;

namespace WebAPI.Application.Services;

public class MoviesService : IMoviesService
{
    private readonly IMovieRepository _repository;
    private readonly IMapper _mapper;
    private readonly IFileStorage _storage;
    private readonly string _container = "movies";

    public MoviesService(IMovieRepository repository, IMapper mapper, IFileStorage storage)
    {
        _repository = repository;
        _mapper = mapper;
        _storage = storage;
    }

    public async Task<IEnumerable<MoviesDto>> GetMovies(MoviesRequestDto request)
    {
        var movies = _repository.GetQuery();

        if (!string.IsNullOrEmpty(request.Title)) 
            movies = movies.Where(m => m.Title.Contains(request.Title));
        
        if (!string.IsNullOrEmpty(request.GenreName))
        {
            movies = movies
                .Where(m => m.MovieGenres
                    .Select(mg => mg.Genre.Name).Contains(request.GenreName));
        }

        if (!string.IsNullOrEmpty(request.Order))
        {
            if (request.Order.ToUpper() == Order.ASC.ToString())
                movies = movies.OrderBy(m => m.PremiereDate);

            if (request.Order.ToUpper() == Order.DESC.ToString())
                movies = movies.OrderByDescending(m => m.PremiereDate);
        }

        return await movies
            .ProjectTo<MoviesDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<MovieDto> GetMovieDetails(int id)
    {
        var movie = await _repository.GetByIdAsync(predicate: m => m.Id == id);

        if (movie is null) throw new KeyNotFoundException();

        return _mapper.Map<MovieDto>(movie);
    }

    public async Task<int> CreateMovie(CreateMovieDto movieDto)
    {
        var movie = _mapper.Map<Movie>(movieDto);
        
        if (movieDto.Poster is not null)
        {
            using (var memoryStream = new MemoryStream())
            {
                await movieDto.Poster.CopyToAsync(memoryStream);
                var content = memoryStream.ToArray();
                var extension = Path.GetExtension(movieDto.Poster.FileName);
                movie.Poster = await _storage.SaveFile(content, extension, _container,
                    movieDto.Poster.ContentType);
            }
        }
        
        _repository.Create(movie);
        await _repository.SaveChangesAsync();

        return movie.Id;
    }

    public async Task UpdateMovie(int id, UpdateMovieDto movieDto)
    {
        var movie = await _repository.GetByIdAsync(predicate: m => m.Id == id);

        if (movie is null) throw new KeyNotFoundException();

        movie = _mapper.Map(movieDto, movie);
        
        if (movieDto.Poster is not null)
        {
            using (var memoryStream = new MemoryStream())
            {
                await movieDto.Poster.CopyToAsync(memoryStream);
                var content = memoryStream.ToArray();
                var extension = Path.GetExtension(movieDto.Poster.FileName);
                movie.Poster = await _storage.SaveFile(content, extension, _container,
                    movieDto.Poster.ContentType);
            }
        }

        _repository.Update(movie);
        await _repository.SaveChangesAsync();
    }

    public async Task DeleteMovie(int id)
    {
        var movie = await _repository.GetByIdAsync(predicate: m => m.Id == id);

        if (movie is null) throw new KeyNotFoundException();
        
        _repository.Delete(movie);
        await _storage.DeleteFile(movie.Poster, "movies");
        await _repository.SaveChangesAsync();
    }
}
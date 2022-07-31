using AutoMapper;
using WebAPI.Application.DTOs.Genres;
using WebAPI.Application.Interfaces.IExternalServices.IRepositories;
using WebAPI.Application.Interfaces.IExternalServices.IServices;
using WebAPI.Domain.Entities;

namespace WebAPI.Application.Services;

public class GenresService : IGenresService
{
    private readonly IGenreRepository _repository;
    private readonly IMapper _mapper;

    public GenresService(IGenreRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<GenreDto>> GetGenres()
    {
        var genres = await _repository.GetAllAsync();

        return _mapper.Map<IEnumerable<GenreDto>>(genres);
    }

    public async Task<int> CreateGenre(CreateGenreDto genreDto)
    {
        var genre = _mapper.Map<Genre>(genreDto);
        
        _repository.Create(genre);
        await _repository.SaveChangesAsync();

        return genre.Id;
    }

    public async Task UpdateGenre(int id, CreateGenreDto genreDto)
    {
        var genre = await _repository.GetAsync(predicate: g => g.Id == id);

        if (genre is null) throw new KeyNotFoundException();

        genre = _mapper.Map(genreDto, genre);
        
        _repository.Update(genre);
        await _repository.SaveChangesAsync();
    }

    public async Task DeleteGenre(int id)
    {
        var genre = await _repository.GetAsync(predicate: g => g.Id == id);

        if (genre is null) throw new KeyNotFoundException();
        
        _repository.Delete(genre);
        await _repository.SaveChangesAsync();
    }
}
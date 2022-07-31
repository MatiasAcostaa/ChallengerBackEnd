using AutoMapper;
using WebAPI.Application.DTOs.Characters;
using WebAPI.Application.DTOs.Genres;
using WebAPI.Application.DTOs.Movies;
using WebAPI.Domain.Entities;

namespace WebAPI.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        #region Genres

        CreateMap<Genre, GenreDto>();

        CreateMap<CreateGenreDto, Genre>();

        #endregion

        #region Movies

        CreateMap<Movie, MoviesDto>();

        CreateMap<Movie, MovieDto>()
            .ForMember(dto => dto.Genres, ops => ops.MapFrom(MapGenresDto))
            .ForMember(m => m.Characters, ops => ops.MapFrom(MapCharactersDto));
            

        CreateMap<CreateMovieDto, Movie>()
            .ForMember(m => m.Poster, ops => ops.Ignore())
            .ForMember(m => m.MovieGenres, ops => ops.MapFrom(MovieGenresMapping))
            .ForMember(m => m.MovieCharacters, ops => ops.MapFrom(MovieCharactersMapping));

        CreateMap<UpdateMovieDto, Movie>()
            .ForMember(m => m.Poster, ops => ops.Ignore())
            .ForMember(m => m.MovieGenres, ops => ops.MapFrom(MovieGenresMapping))
            .ForMember(m => m.MovieCharacters, ops => ops.MapFrom(MovieCharactersMapping));

        #endregion

        #region Characters

        CreateMap<Character, CharactersDto>();

        CreateMap<Character, CharacterDto>()
            .ForMember(ma => ma.Movies, ops => ops.MapFrom(MoviesMapping));

        CreateMap<CreateCharacterDto, Character>()
            .ForMember(a => a.Image, ops => ops.Ignore());

        #endregion
    }
    
    #region MappingMovie
        
    public IList<GenreDto> MapGenresDto(Movie movie, MovieDto movieDto)
    {
        var result = new List<GenreDto>();

        if (movie.MovieGenres is null)
            return result;
        
        foreach (var genre in movie.MovieGenres)
        {
            result.Add(new GenreDto
            {
                Id = genre.GenreId,
                Name = genre.Genre.Name
            });
        }

        return result;
    }

    public IList<CharactersDto> MapCharactersDto(Movie movie, MovieDto movieDto)
    {
        var result = new List<CharactersDto>();

        if (movie.MovieCharacters is null)
            return result;

        foreach (var character in movie.MovieCharacters)
        {
            result.Add(new CharactersDto
            {
                Id = character.CharacterId,
                Name = character.Character.Name,
                Image = character.Character.Image
            });
        }

        return result;
    }

    public IList<MovieGenre> MovieGenresMapping(CreateMovieDto movieDto, Movie movie)
    {
        var result = new List<MovieGenre>();

        if (movieDto.GenresIds is null) 
            return result;

        foreach (var genreId in movieDto.GenresIds)
            result.Add(new MovieGenre{ GenreId = genreId });

        return result;
    }

    public IList<MovieCharacter> MovieCharactersMapping(CreateMovieDto movieDto, Movie movie)
    {
        var result = new List<MovieCharacter>();

        if (movieDto.CharactersIds is null)
            return result;

        foreach (var actorId in movieDto.CharactersIds)
            result.Add(new MovieCharacter { CharacterId = actorId});
        

        return result;
    }

    public IList<MovieGenre> MovieGenresMapping(UpdateMovieDto movieDto, Movie movie)
    {
        var result = new List<MovieGenre>();
        
        if (movieDto.GenresIds is null) 
            return result;

        foreach (var genre in movieDto.GenresIds)
            result.Add(new  MovieGenre { GenreId = genre});

        return result;
    }

    private IList<MovieCharacter> MovieCharactersMapping(UpdateMovieDto movieDto, Movie movie)
    {
        var result = new List<MovieCharacter>();

        if (movieDto.CharactersIds is null)
            return result;

        foreach (var actor in movieDto.CharactersIds)
            result.Add(new MovieCharacter{ CharacterId = actor});
        

        return result;
    }


    #endregion


    #region MappingCharacter

    private IList<MoviesDto> MoviesMapping(Character character, CharacterDto characterDto)
    {
        var result = new List<MoviesDto>();

        if (character.MovieCharacters is null) 
            return result;

        foreach (var movie in character.MovieCharacters)
            result.Add(new MoviesDto
            {
                Id = movie.MovieId,
                Title = movie.Movie.Title,
                Poster = movie.Movie.Poster,
                PremiereDate = movie.Movie.PremiereDate
            });
        
        
        return result;
    }

    #endregion
}
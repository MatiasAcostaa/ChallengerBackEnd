using FluentValidation;
using WebAPI.Application.DTOs.Characters;
using WebAPI.Application.DTOs.Genres;

namespace WebAPI.Application.DTOs.Movies;

public class MovieDto
{
    public string Title { get; set; } = null!;
    public DateTime PremiereDate { get; set; }
    public string Poster { get; set; } = null!;
    public int Rating { get; set; }
    public List<GenreDto> Genres { get; set; } = null!;
    public List<CharactersDto> Characters { get; set; } = null!;
    
    public class MovieDtoValidator : AbstractValidator<MovieDto>
    {
        public MovieDtoValidator()
        {
            RuleFor(m => m.Title)
                .NotEmpty()
                .MaximumLength(100);
            
            RuleFor(m => m.Rating)
                .NotNull();

            RuleFor(m => m.Genres)
                .NotNull();

            RuleFor(m => m.Characters)
                .NotNull();
        }
    }
}
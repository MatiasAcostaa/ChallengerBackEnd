using FluentValidation;

namespace WebAPI.Application.DTOs.Movies;

public class CreateMovieDto
{
    public string Title { get; set; } = null!;
    public DateTime PremiereDate { get; set; }
    public IFormFile Poster { get; set; } = null!;
    public int Rating { get; set; }

    public List<int> GenresIds { get; set; } = null!;

    public List<int> CharactersIds { get; set; } = null!;
}

public class CreateMovieDtoValidator : AbstractValidator<CreateMovieDto>
{
    public CreateMovieDtoValidator()
    {
        RuleFor(m => m.Title)
            .MaximumLength(100)
            .NotEmpty();

        RuleFor(m => m.Rating)
            .NotNull();
        
        RuleFor(m => m.GenresIds)
            .NotNull();
        
        RuleFor(m => m.CharactersIds)
            .NotNull();
        
        RuleFor(p => p.Poster.Length)
            .LessThanOrEqualTo(10485760).WithMessage("File size is bigger than 10 MB.");
        RuleFor(p => p.Poster.ContentType)
            .Must(contentType => contentType.Equals("image/jpeg") || contentType.Equals("image/png"))
            .WithMessage("File extension is not valid.");
        
        
    }
}
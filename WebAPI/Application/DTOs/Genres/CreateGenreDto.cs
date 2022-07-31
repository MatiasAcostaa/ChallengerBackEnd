using FluentValidation;

namespace WebAPI.Application.DTOs.Genres;

public class CreateGenreDto
{
    public string Name { get; init; } = null!;
}


public class CreateGenreDtoValidator : AbstractValidator<CreateGenreDto>
{
    public CreateGenreDtoValidator()
    {
        RuleFor(g => g.Name)
            .MaximumLength(50)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");
    }
}



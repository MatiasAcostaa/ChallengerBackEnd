using FluentValidation;

namespace WebAPI.Application.DTOs.Characters;

public class CharactersDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    
    public string Image { get; set; } = null!;
}

public class CharactersDtoValidator : AbstractValidator<CharactersDto>
{
    public CharactersDtoValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(100);
    }
}
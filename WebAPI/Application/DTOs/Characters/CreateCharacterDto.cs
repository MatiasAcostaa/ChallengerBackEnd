using FluentValidation;

namespace WebAPI.Application.DTOs.Characters;

public class CreateCharacterDto
{
    public string Name { get; set; } = null!;

    public int Age { get; set; }
    public IFormFile Image { get; set; } = null!;
    
}

public class CreateCharacterDtoValidator : AbstractValidator<CreateCharacterDto>
{
    public CreateCharacterDtoValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(100);
        
        RuleFor(p => p.Image.Length)
            .LessThanOrEqualTo(10485760).WithMessage("File size is bigger than 10 MB.");
        RuleFor(p => p.Image.ContentType)
            .Must(contentType => contentType.Equals("image/jpeg") || contentType.Equals("image/png"))
            .WithMessage("File extension is not valid.");
    }
}
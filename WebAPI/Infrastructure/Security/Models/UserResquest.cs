using FluentValidation;

namespace WebAPI.Infrastructure.Security.Models;

public class UserResquest
{
    public string Email { get; init; } = null!;

    public string Password { get; init; } = null!;
}

public class UserResquestValidator : AbstractValidator<UserResquest>
{
    public UserResquestValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(u => u.Password)
            .NotEmpty();
    }
}
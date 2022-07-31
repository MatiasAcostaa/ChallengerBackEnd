using WebAPI.Application.DTOs.Movies;

namespace WebAPI.Application.DTOs.Characters;

public class CharacterDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Age { get; set; }
    public string Image { get; set; } = null!;
    public List<MoviesDto> Movies { get; set; } = null!;
}
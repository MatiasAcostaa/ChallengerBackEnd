namespace WebAPI.Domain.Entities;

public class Character
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Age { get; set; }
    public string Image { get; set; } = null!;
    public List<MovieCharacter> MovieCharacters { get; set; } = null!;
}
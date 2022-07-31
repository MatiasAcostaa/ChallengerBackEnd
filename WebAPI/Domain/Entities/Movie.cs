namespace WebAPI.Domain.Entities;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime PremiereDate { get; set; }
    public string Poster { get; set; } = null!;
    public int Rating { get; set; }
    public List<MovieCharacter> MovieCharacters { get; set; } = null!;
    public List<MovieGenre> MovieGenres { get; set; } = null!;
}
namespace WebAPI.Application.DTOs.Movies;

public class MoviesDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Poster { get; set; } = null!;
    public DateTime PremiereDate { get; set; }
}
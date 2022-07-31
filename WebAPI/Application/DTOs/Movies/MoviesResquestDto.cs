namespace WebAPI.Application.DTOs.Movies;

public record MoviesRequestDto(string? Title, string? GenreName, string? Order);
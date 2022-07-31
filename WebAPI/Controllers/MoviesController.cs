using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Application.DTOs.Movies;
using WebAPI.Application.Interfaces.IExternalServices.IServices;

namespace WebAPI.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/movies")]
public class MoviesController : ControllerBase
{
    private readonly IMoviesService _service;

    public MoviesController(IMoviesService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetMovies([FromQuery] MoviesRequestDto request) =>
        Ok(await _service.GetMovies(request));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetMovieDetails(int id) =>
        Ok(await _service.GetMovieDetails(id));

    [HttpPost]
    public async Task<IActionResult> CreateMovie([FromForm] CreateMovieDto movieDto) =>
        Ok(await _service.CreateMovie(movieDto));

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutMovie(int id,[FromForm] UpdateMovieDto movieDto)
    {
        await _service.UpdateMovie(id, movieDto);

        return StatusCode(204);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        await _service.DeleteMovie(id);

        return StatusCode(204);
    }
}
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Application.DTOs.Genres;
using WebAPI.Application.Interfaces.IExternalServices.IServices;

namespace WebAPI.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/genres")]
public class GenresController : ControllerBase
{
    private readonly IGenresService _service;

    public GenresController(IGenresService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetGenres() => Ok(await _service.GetGenres());

    [HttpPost]
    public async Task<IActionResult> PostGenre([FromBody] CreateGenreDto genreDto) =>
        Ok(await _service.CreateGenre(genreDto));

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutGenre(int id, [FromBody] CreateGenreDto genreDto)
    {
        await _service.UpdateGenre(id, genreDto);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteGenre(int id)
    {
        await _service.DeleteGenre(id);

        return NoContent();
    }
}
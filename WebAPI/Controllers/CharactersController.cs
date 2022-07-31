using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Application.DTOs.Characters;
using WebAPI.Application.Interfaces.IExternalServices.IServices;

namespace WebAPI.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/characters")]
public class CharactersController : ControllerBase
{
    private readonly ICharactersService _service;

    public CharactersController(ICharactersService service)
    {
        _service = service;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetCharacter([FromQuery] CharactersResquestDto resquest) => 
        Ok(await _service.GetCharacters(resquest));
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCharacterById(int id) =>
        Ok(await _service.GetCharacterDetails(id));
    
    [HttpPost]
    public async Task<IActionResult> CreateCharacter([FromForm] CreateCharacterDto characterDto) =>
        Ok(await _service.CreateCharacter(characterDto));
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutCharacter(int id, [FromForm] CreateCharacterDto characterDto)
    {
        await _service.UpdateCharacter(id, characterDto);

        return NoContent();
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCharacter(int id)
    {
        await _service.DeleteCharacter(id);

        return NoContent();
    }
}
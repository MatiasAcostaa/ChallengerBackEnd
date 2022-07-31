using WebAPI.Application.DTOs.Characters;

namespace WebAPI.Application.Interfaces.IExternalServices.IServices;

public interface ICharactersService
{
    Task<IEnumerable<CharactersDto>> GetCharacters(CharactersResquestDto resquest);

    Task<CharacterDto> GetCharacterDetails(int id);

    Task<int> CreateCharacter(CreateCharacterDto characterDto);

    Task UpdateCharacter(int id, CreateCharacterDto characterDto);

    Task DeleteCharacter(int id);
}
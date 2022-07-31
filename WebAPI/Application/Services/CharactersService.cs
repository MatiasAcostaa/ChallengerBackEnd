using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WebAPI.Application.DTOs.Characters;
using WebAPI.Application.Interfaces.IExternalServices;
using WebAPI.Application.Interfaces.IExternalServices.IRepositories;
using WebAPI.Application.Interfaces.IExternalServices.IServices;
using WebAPI.Domain.Entities;

namespace WebAPI.Application.Services;

public class CharactersService : ICharactersService
{
    private readonly ICharacterRepository _repository;
    private readonly IMapper _mapper;
    private readonly IFileStorage _storage;
    private readonly string _container = "actors";

    public CharactersService(ICharacterRepository repository, IMapper mapper, IFileStorage storage)
    {
        _repository = repository;
        _mapper = mapper;
        _storage = storage;
    }
    public async Task<IEnumerable<CharactersDto>> GetCharacters(CharactersResquestDto resquest)
    {
        var characters = _repository.GetQuery();

        if (!string.IsNullOrEmpty(resquest.Name))
            characters = characters.Where(a => a.Name.Contains(resquest.Name));

        if (!string.IsNullOrEmpty(resquest.MovieName))
        {
            characters = characters
                .Where(a => a.MovieCharacters!
                    .Select(ma => ma.Movie.Title).Contains(resquest.MovieName));
        }

        return await characters
            .ProjectTo<CharactersDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

    }
    
    public async Task<CharacterDto> GetCharacterDetails(int id)
    {
        var character = await _repository.GetByIdAsync(predicate: a => a.Id == id);

        if (character is null) throw new KeyNotFoundException();

        return _mapper.Map<CharacterDto>(character);
    }

    public async Task<int> CreateCharacter(CreateCharacterDto characterDto)
    {
        var character = _mapper.Map<Character>(characterDto);

        if (characterDto.Image is not null)
        {
            using (var memoryStream = new  MemoryStream())
            {
                await characterDto.Image.CopyToAsync(memoryStream);
                var content = memoryStream.ToArray();
                var extension = Path.GetExtension(characterDto.Image.FileName);
                character.Image = await _storage.SaveFile(content, extension, _container,
                    characterDto.Image.ContentType);
            }
        }
        
        _repository.Create(character);
        await _repository.SaveChangesAsync();

        return character.Id;
    }

    public async Task UpdateCharacter(int id, CreateCharacterDto characterDto)
    {
        var character = await _repository.GetByIdAsync(predicate: a => a.Id == id);

        if (character is null) throw new KeyNotFoundException();

        character = _mapper.Map(characterDto, character);
        
        if (characterDto.Image is not null)
        {
            using (var memoryStream = new  MemoryStream())
            {
                await characterDto.Image.CopyToAsync(memoryStream);
                var content = memoryStream.ToArray();
                var extension = Path.GetExtension(characterDto.Image.FileName);
                character.Image = await _storage.SaveFile(content, extension, _container,
                    characterDto.Image.ContentType);
            }
        }
        
        _repository.Update(character);
        await _repository.SaveChangesAsync();
    }

    public async Task DeleteCharacter(int id)
    {
        var character = await _repository.GetByIdAsync(predicate: a => a.Id == id);

        if (character is null) throw new KeyNotFoundException();
        
        _repository.Delete(character);
        await _storage.DeleteFile(character.Image, "actors");
        await _repository.SaveChangesAsync();
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using WebAPI.Application.DTOs.Genres;
using WebAPI.Application.Interfaces.IExternalServices.IRepositories;
using WebAPI.Application.Interfaces.IExternalServices.IServices;
using WebAPI.Application.Mappings;
using WebAPI.Application.Services;
using WebAPI.Domain.Entities;

namespace UnitTest;

public class GenresServiceTest
{
    private readonly Mock<IGenreRepository> _repository = new();
    private readonly MapperConfiguration _configuration = new(configure: config => config.AddProfile<MappingProfile>());

    public GenresServiceTest()
    {
        Service = new GenresService(_repository.Object, _configuration.CreateMapper());
    }
    
    private IGenresService Service { get; }

    #region GET ALL

    [Fact]
    public async Task GetAll_SiExistenGeneros_RetornaTodosLosGeneros()
    {
        //Arrange
        var genres = new List<Genre>
        {
            new() {Id = 1, Name = "Acción"},
            new() {Id = 2, Name = "Drama"},
            new() {Id = 3, Name = "Terror"}
        };

        _repository.Setup(r => r.GetAllAsync()).ReturnsAsync(genres);
        
        //Act
        var service = await Service.GetGenres();
        
        //Assert
        Assert.IsType<List<GenreDto>>(service);
        Assert.Equal(genres.Count, service.Count());
    }

    #endregion

    #region CREATE

    [Fact]
    public async Task Create_ConGeneroParaCrear_RetornaGeneroCreado()
    {
        //Arrange
        var genres = new CreateGenreDto
        {
            Name = "Aventura"
        };
        
        _repository.Setup(r => r.Create(It.IsAny<Genre>())).Verifiable("Creado");
        _repository.Setup(r => r.SaveChangesAsync());
        
        //Act
        var service = await Service.CreateGenre(genres);
        
        //Assert
        Assert.Equal(new int(), service);


    }
    
    #endregion

    #region UPDATE

    [Fact]
    public async Task Update_ConGenerosExistentes_NoHayDevoluciones()
    {
        // Arrange
        const int id = 1;
        
        var genres = new List<Genre>
        {
            new() {Id = 1, Name = "Acción"},
            new() {Id = 2, Name = "Drama"},
            new() {Id = 3, Name = "Terror"}
        };

        var genreDto = new CreateGenreDto {Name = "Ciencia Ficción"};

        _repository.Setup(r => r.GetAsync(g => g.Id == id)).ReturnsAsync(genres.FirstOrDefault(g => g.Id == id));
        _repository.Setup(r => r.Update(It.IsAny<Genre>()));
        _repository.Setup(r => r.SaveChangesAsync());
        
        // Act
        await Service.UpdateGenre(id, genreDto);
        
        // Assert
        _repository.Verify(r => r.Update(It.Is<Genre>(g => g.Id == id && g.Name == genreDto.Name)), Times.Once);
    }

    [Fact]
    public async Task Update_ConGeneroInexistente_NotFound()
    {
        //Arrage
        var genres = new List<Genre> {new() {Id = 1, Name = "Fantasia"}};

        const int id = 2;

        var genreDto = new CreateGenreDto { Name = "Accion" };

        _repository.Setup(r => r.GetByIdAsync(g => g.Id == id)).ReturnsAsync(genres.FirstOrDefault(g => g.Id == id));
        //Act

        //Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => Service.UpdateGenre(id, genreDto));
    }

    #endregion

    #region DELETE

    [Fact]
    public async Task Delete_ConGenerosExistente_NoReturns()
    {
        //Arrange
        var genres = new List<Genre>
        {
            new() {Id = 1, Name = "Acción"},
            new() {Id = 2, Name = "Drama"},
            new() {Id = 3, Name = "Terror"}
        };

        const int id = 1;
        
        _repository.Setup(r => r.GetAsync(g => g.Id == id)).ReturnsAsync(genres.FirstOrDefault(g => g.Id == id));
        _repository.Setup(r => r.Delete(It.IsAny<Genre>())).Callback<Genre>(entity => genres.Remove(entity));
        _repository.Setup(r => r.SaveChangesAsync());
        
        //Act
        await Service.DeleteGenre(id);

        //Assert
         Assert.Equal(2, genres.Count);
         _repository.Verify(r => r.GetAsync(g => g.Id == id), Times.Once());
         _repository.Verify(r => r.Delete(It.Is<Genre>(g => g != null)), Times.Once);
    }

    [Fact]
    public async Task Delete_ConGenerosInexistente_NotFound()
    {
        //Arrange
        var genres = new List<Genre> {new() { Id = 1, Name = "Accion" }};

        const int id = 2;

        _repository.Setup(r => r.GetAsync(g => g.Id == id)).ReturnsAsync(genres.SingleOrDefault(g => g.Id == id));
        _repository.Setup(r => r.Delete(It.IsAny<Genre>())).Callback<Genre>(entity => genres.Remove(entity));
        _repository.Setup(r => r.SaveChangesAsync());

        //Act

        //Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => Service.DeleteGenre(id));
    }

    #endregion
}
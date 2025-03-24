using DeletarContato.API.Controllers;
using DeletarContato.Application.Services;
using DeletarContato.Domain.Domain;
using DeletarContato.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace DeletarContato.API.Test.Controllers;

public class DeletarContatoControllerTest
{
    private readonly DeletarContatoController _controller;
    private readonly ContactZoneDbContext _dbContext;

    public DeletarContatoControllerTest()
    {
        var options = new DbContextOptionsBuilder<ContactZoneDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Cria um DB novo para cada teste
            .Options;

        _dbContext = new ContactZoneDbContext(options);
        Mock<IContatoService> contatoServiceMock = new();

        _controller = new DeletarContatoController(_dbContext, contatoServiceMock.Object);
    }

    [Fact]
    public void EnviarContatoParaDeletar_DeveRetornarOk()
    {
        // Arrange
        int contatoId = 1;

        // Act
        var resultado = _controller.EnviarContatoParaDeletar(contatoId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(resultado);
        Assert.Equal("Contato enviado para a fila para ser deletado.", okResult.Value);
    }

    [Fact]
    public async Task DeletarContato_ContatoNaoEncontrado_DeveRetornarNotFound()
    {
        // Arrange
        int contatoId = 1;

        // Act
        var resultado = await _controller.DeletarContato(contatoId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(resultado);
        Assert.Equal("Contato não encontrado.", notFoundResult.Value);
    }

    [Fact]
    public async Task GetAllContatos_DeveRetornarListaContatos()
    {
        // Arrange
        _dbContext.Contatos.Add(new ContactDomain 
        { 
            Id = 1, 
            Name = "Teste 1", 
            Email = "teste1@email.com",
            DDD = "11",
            Phone = "999999999"
        });

        _dbContext.Contatos.Add(new ContactDomain 
        { 
            Id = 2, 
            Name = "Teste 2", 
            Email = "teste2@email.com",
            DDD = "21",
            Phone = "988888888"
        });

        await _dbContext.SaveChangesAsync(); // Salvar no banco em memória

        // Act
        var resultado = await _controller.GetAllContatos();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(resultado);
        var resultList = Assert.IsType<List<ContactDomain>>(okResult.Value);
        Assert.Equal(2, resultList.Count);
    }

}
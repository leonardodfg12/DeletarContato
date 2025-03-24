
using DeletarContato.Application.Services;
using DeletarContato.Domain.Domain;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;

namespace DeletarContato.Application.Test.Services
{
    public class ContatoServiceTest
    {
        private readonly Mock<IBus> _busMock;
        private readonly Mock<ILogger<ContatoService>> _loggerMock;
        private readonly ContatoService _contatoService;

        public ContatoServiceTest()
        {
            _busMock = new Mock<IBus>();
            _loggerMock = new Mock<ILogger<ContatoService>>();
            _contatoService = new ContatoService(_busMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task EnviarContatoParaFila_ComContatoValido_DeveEnviarMensagem()
        {
            // Arrange
            var contato = new ContactDomain { Id = 1 };
            var sendEndpointMock = new Mock<ISendEndpoint>();

            _busMock
                .Setup(b => b.GetSendEndpoint(It.IsAny<Uri>()))
                .ReturnsAsync(sendEndpointMock.Object);

            // Act
            _contatoService.EnviarContatoParaFila(contato);
            await Task.Delay(100); // Aguarda a execução do método async

            // Assert
            _busMock.Verify(b => b.GetSendEndpoint(It.Is<Uri>(uri => uri.ToString() == "queue:deletar-contato-queue")), Times.Once);
            sendEndpointMock.Verify(e => e.Send(It.Is<ContactDomain>(c => c.Id == 1), default), Times.Once);
            _loggerMock.Verify(l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Message sent successfully")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }

        [Fact]
        public void EnviarContatoParaFila_ComContatoNulo_DeveLogarAvisoENaoEnviarMensagem()
        {
            // Act
            _contatoService.EnviarContatoParaFila(null);

            // Assert
            _loggerMock.Verify(l => l.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("ContatoDto is null")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);

            _busMock.Verify(b => b.GetSendEndpoint(It.IsAny<Uri>()), Times.Never);
        }
    }
}
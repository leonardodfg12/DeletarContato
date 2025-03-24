using DeletarContato.Domain.Domain;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace DeletarContato.Application.Services
{
    public class ContatoService : IContatoService
    {
        private readonly IBus _bus;
        private readonly ILogger<ContatoService> _logger;

        public ContatoService(IBus bus, ILogger<ContatoService> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        public async void EnviarContatoParaFila(ContactDomain? contatoDto)
        {
            if (contatoDto == null)
            {
                _logger.LogWarning("ContatoDto is null");
                return;
            }

            var mensagem = new ContactDomain
            {
                Id = contatoDto.Id
            };

            _logger.LogInformation("Sending message to queue: deletar-contato-queue with Id: {MessageId}", mensagem.Id);

            var endpoint = await _bus.GetSendEndpoint(new Uri("queue:deletar-contato-queue"));
            await endpoint.Send(mensagem);

            _logger.LogInformation("Message sent successfully: {MessageId}", mensagem.Id);
        }
    }
}
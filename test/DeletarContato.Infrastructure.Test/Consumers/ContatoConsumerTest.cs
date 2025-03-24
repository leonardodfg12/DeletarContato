using DeletarContato.Domain.Domain;
using DeletarContato.Infrastructure.Consumers;
using DeletarContato.Infrastructure.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace DeletarContato.Infrastructure.Test.Consumers
{
    public class ContatoConsumerTests : IDisposable
    {
        private readonly ContactZoneDbContext _context;
        private readonly Mock<ConsumeContext<ContactDomain>> _mockConsumeContext;
        private readonly ContatoConsumer _consumer;

        public ContatoConsumerTests()
        {
            var options = new DbContextOptionsBuilder<ContactZoneDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ContactZoneDbContext(options);
            Mock<ILogger<ContatoConsumer>> mockLogger = new();
            _mockConsumeContext = new Mock<ConsumeContext<ContactDomain>>();
            _consumer = new ContatoConsumer(_context, mockLogger.Object);
        }
        
        [Fact]
        public async Task Consume_ComContatoExistente_DeveDeletarContatoESalvarAlteracoes()
        {
            var contatoId = 1;
            var contatoMessage = new ContactDomain { Id = contatoId };
            var contatoExistente = new ContactDomain { Id = contatoId, DDD = "11", Email = "test@example.com", Name = "Test Name", Phone = "123456789" };

            _context.Contatos.Add(contatoExistente);
            await _context.SaveChangesAsync();

            _mockConsumeContext.Setup(x => x.Message).Returns(contatoMessage);

            await _consumer.Consume(_mockConsumeContext.Object);

            var contatoRemovido = await _context.Contatos.FindAsync(contatoId);
            Assert.Null(contatoRemovido);
            
        }
        
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
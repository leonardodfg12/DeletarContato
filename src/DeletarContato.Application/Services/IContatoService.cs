using DeletarContato.Domain.Domain;

namespace DeletarContato.Application.Services
{
    public interface IContatoService
    {
        void EnviarContatoParaFila(ContactDomain? contato);
    }
}
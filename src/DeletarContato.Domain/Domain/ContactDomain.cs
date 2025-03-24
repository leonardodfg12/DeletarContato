using System.Diagnostics.CodeAnalysis;

namespace DeletarContato.Domain.Domain
{
    [ExcludeFromCodeCoverage]
    public class ContactDomain
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DDD { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
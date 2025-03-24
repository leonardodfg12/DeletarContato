using System.Diagnostics.CodeAnalysis;
using DeletarContato.Domain.Domain;
using DeletarContato.Infrastructure.Data.FluentMap;
using Microsoft.EntityFrameworkCore;

namespace DeletarContato.Infrastructure.Data
{
    [ExcludeFromCodeCoverage]
    public class ContactZoneDbContext : DbContext
    {
        public DbSet<ContactDomain> Contatos { get; set; }

        public ContactZoneDbContext(DbContextOptions<ContactZoneDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ContactMap());
        }
    }
}
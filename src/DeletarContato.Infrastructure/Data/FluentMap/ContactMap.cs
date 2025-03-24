using System.Diagnostics.CodeAnalysis;
using DeletarContato.Domain.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeletarContato.Infrastructure.Data.FluentMap
{
    [ExcludeFromCodeCoverage]
    public class ContactMap : IEntityTypeConfiguration<ContactDomain>
    {
        public void Configure(EntityTypeBuilder<ContactDomain> builder)
        {
            builder.ToTable("Contact");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(x => x.Name)
               .IsRequired()
               .HasColumnName("Name")
               .HasColumnType("NVARCHAR")
               .HasMaxLength(80);

            builder.Property(x => x.DDD)
               .IsRequired()
               .HasColumnName("DDD")
               .HasColumnType("NVARCHAR")
               .HasMaxLength(3);

            builder.Property(x => x.Phone)
               .IsRequired()
               .HasColumnName("Phone")
               .HasColumnType("NVARCHAR")
               .HasMaxLength(11);

            builder.Property(x => x.Email)
               .IsRequired()
               .HasColumnName("Email")
               .HasColumnType("NVARCHAR")
               .HasMaxLength(200);

        }
    }
}

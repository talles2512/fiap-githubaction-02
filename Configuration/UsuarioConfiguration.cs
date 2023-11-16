using FiapStore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiapStore.Configuration
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuario");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                .HasColumnType("int")
                .UseIdentityColumn();
            builder.Property(u => u.Nome).HasColumnType("varchar(100)");
            builder.Property(u => u.NomeUsuario)
                .HasColumnType("varchar(50)")
                .IsRequired();
            builder.Property(u => u.Senha)
                .HasColumnType("varchar(50)")
                .IsRequired();
            builder.Property(u => u.Permissao)
                .HasConversion<int>() //string caso queiramos salvar o conteúdo
                .IsRequired();
            builder.HasMany(u => u.Pedidos)
                .WithOne(u => u.Usuario)
                .HasForeignKey(u => u.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

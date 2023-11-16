using FiapStore.Configuration;
using FiapStore.Entities;
using Microsoft.EntityFrameworkCore;

namespace FiapStore.Repositories
{
    public class ApplicationDbContext: DbContext
    {
        private readonly IConfiguration _configuration;
        public ApplicationDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        DbSet<Usuario> Usuarios { get; set; }
        DbSet<Pedido> Pedidos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(_configuration.GetConnectionString("ConnectionString"));
            optionsBuilder.UseInMemoryDatabase("dbTemp");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
            modelBuilder.ApplyConfiguration(new PedidoConfiguration());
        }
    }
}

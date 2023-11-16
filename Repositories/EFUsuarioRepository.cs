using FiapStore.Entities;
using FiapStore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiapStore.Repositories
{
    public class EFUsuarioRepository : EFRepository<Usuario>, IUsuarioRepository
    {
        public EFUsuarioRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Usuario ObterComPedidosPorId(int id)
        {
            //return _dbSet
            //    .Include(x => x.Pedidos)
            //    .Where(x => x.Id == id)
            //    .ToList()
            //    .Select(usuario =>
            //    {
            //        usuario.Pedidos = usuario.Pedidos.Select(pedido => new Pedido(pedido)).ToList();
            //        return usuario;
            //    }).FirstOrDefault();

            return _dbSet
                .Include(x => x.Pedidos)
                .FirstOrDefault(x => x.Id == id);
        }

        public Usuario ObterPorNomeUsuarioESenha(string nomeUsuario, string senha)
        {
            return _dbSet
                .FirstOrDefault(x => x.NomeUsuario == nomeUsuario && x.Senha == senha);
        }
    }
}

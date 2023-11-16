using FiapStore.Entities;

namespace FiapStore.Interfaces
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Usuario ObterComPedidosPorId(int id);
        Usuario ObterPorNomeUsuarioESenha(string nomeUsuario, string senha);
    }
}

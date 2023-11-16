using Dapper;
using FiapStore.Entities;
using FiapStore.Interfaces;
using System.Data.SqlClient;

namespace FiapStore.Repositories
{
    public class UsuarioRepository : DapperRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public override void Alterar(Usuario entidade)
        {
            using var conn = new SqlConnection(ConnectionString);
            var query = "UPDATE Usuario SET Nome = @Nome WHERE Id = @Id";
            conn.Query(query, entidade);
        }

        public override void Cadastrar(Usuario entidade)
        {
            using var conn = new SqlConnection(ConnectionString);
            var query = "INSERT INTO Usuario(Nome) VALUES (@Nome)";
            conn.Execute(query, entidade);
        }

        public override void Deletar(int id)
        {
            using var conn = new SqlConnection(ConnectionString);
            var query = "DELETE FROM Usuario WHERE Id = @Id";
            conn.Execute(query, new { Id = id });
        }

        public override Usuario ObterPorId(int id)
        {
            using var conn = new SqlConnection(ConnectionString);
            var query = "SELECT Id, Nome FROM Usuario WHERE Id = @Id";
            return conn.Query<Usuario>(query, new { Id = id}).FirstOrDefault();
        }

        public override IList<Usuario> ObterTodos()
        {
            using var conn = new SqlConnection(ConnectionString);
            var query = "SELECT Id, Nome FROM Usuario";
            return conn.Query<Usuario>(query).ToList();
        }

        public Usuario ObterComPedidosPorId(int id)
        {
            using var conn = new SqlConnection(ConnectionString);
            var query = "SELECT U.Id, U.Nome, P.Id, P.NomeProduto, P.UsuarioId FROM Usuario U LEFT JOIN Pedido P ON U.Id = P.UsuarioId WHERE U.Id = @Id";
            var resultado = new Dictionary<int, Usuario>();

            conn.Query<Usuario, Pedido, Usuario>(query, (usuario, pedido) =>
            {
                if(!resultado.TryGetValue(usuario.Id, out var usuarioExistente))
                {
                    usuarioExistente = usuario;
                    usuarioExistente.Pedidos = new List<Pedido>();
                    resultado.Add(usuario.Id, usuarioExistente);
                }

                if(pedido != null)
                {
                    usuarioExistente.Pedidos.Add(pedido);
                }

                return usuarioExistente;
            }, new { Id = id }, splitOn: "Id");

            return resultado.Values.FirstOrDefault();
        }

        public Usuario ObterPorNomeUsuarioESenha(string nomeUsuario, string senha)
        {
            throw new NotImplementedException();
        }
    }
}

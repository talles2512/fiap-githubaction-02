using FiapStore.DTOs;
using FiapStore.Enum;

namespace FiapStore.Entities
{
    public class Usuario : Entidade
    {
        public string Nome { get; set; }
        public string NomeUsuario { get; set; }
        public string Senha { get; set; }
        public TipoPermissao Permissao { get; set; }
        public ICollection<Pedido> Pedidos { get; set; }

        public Usuario()
        {
            
        }

        public Usuario(CadastrarUsuarioDTO cadastrarUsuarioDTO)
        {
            Nome = cadastrarUsuarioDTO.Nome;
            NomeUsuario = cadastrarUsuarioDTO.NomeUsuario;
            Senha = cadastrarUsuarioDTO.Senha;
            Permissao = cadastrarUsuarioDTO.Permissao;
        }

        public Usuario(AlterarUsuarioDTO alterarUsuarioDTO)
        {
            Id = alterarUsuarioDTO.Id;
            Nome = alterarUsuarioDTO.Nome;
        }
    }
}
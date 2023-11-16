using FiapStore.DTOs;
using FiapStore.Entities;
using FiapStore.Enum;
using FiapStore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiapStore.Controllers
{
    [ApiController]
    [Route("usuarios")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(IUsuarioRepository usuarioRepository, ILogger<UsuarioController> logger)
        {
            _usuarioRepository = usuarioRepository;
            _logger = logger;
        }

        [Authorize]
        [Authorize(Roles = Permissoes.Administrador)]
        [HttpGet("obter-todos-usuarios")]
        public IActionResult ObterTodosUsuarios()
        {
            _logger.LogInformation($"{typeof(UsuarioController).Name}:ObterTodosUsuarios => obtendo todos os usuários");
            return Ok(_usuarioRepository.ObterTodos());
        }

        [Authorize]
        [HttpGet("obter-por-usuario-id/{id}")]
        public IActionResult ObterPorUsuarioId(int id)
        {
            return Ok(_usuarioRepository.ObterPorId(id));
        }

        [Authorize]
        [Authorize(Roles = Permissoes.Funcionario)]
        [HttpGet("obter-com-pedidos-por-usuario-id/{id}")]
        public IActionResult ObterComPedidosPorUsuarioId(int id)
        {
            return Ok(_usuarioRepository.ObterComPedidosPorId(id));
        }

        [Authorize]
        [Authorize(Roles = $"{Permissoes.Funcionario},{Permissoes.Administrador}")]
        [HttpPost("cadastrar-usuario")]
        public IActionResult CadastrarUsuario(CadastrarUsuarioDTO usuario)
        {
            _usuarioRepository.Cadastrar(new(usuario));

            return Ok("Usuário cadastrado com sucesso");
        }

        [HttpPut("alterar-usuario")]
        public IActionResult AlterarUsuario(AlterarUsuarioDTO usuario)
        {
            var usuarioDb = _usuarioRepository.ObterPorId(usuario.Id);

            if (usuarioDb == null)
                return NotFound("Usuário não encontrado");

            usuarioDb.Nome = usuario.Nome;

            _usuarioRepository.Alterar(usuarioDb);

            return Ok("Usuario alterado com sucesso");
        }

        [HttpDelete("deletar-usuario/{id}")]
        public IActionResult DeletarUsuario(int id)
        {
            _usuarioRepository.Deletar(id);

            return Ok("Usuario deletado com sucesso");
        }
    }
}

using FiapStore.DTOs;
using FiapStore.Interfaces;
using FiapStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace FiapStore.Controllers
{
    [ApiController]
    [Route("login")]
    public class LoginController: ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenService _tokenService;

        public LoginController(IUsuarioRepository usuarioRepository, ITokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Método para gerar token de autenticação do Usuário
        /// </summary>
        /// <param name="loginDTO"></param>
        /// <returns>Retorna o usuário e token respectivos</returns>
        /// <remarks>
        /// 
        /// Enviar login e senha
        /// </remarks>
        /// <response code="200">Retorna sucesso</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="403">Proibído</response>
        [HttpPost]
        public IActionResult Autenticar([FromBody] LoginDTO loginDTO)
        {
            var usuario = _usuarioRepository.ObterPorNomeUsuarioESenha(loginDTO.NomeUsuario, loginDTO.Senha);

            if (usuario is null)
                return NotFound(new { mensagem = "Usuário ou senha inválidos" });

            var token = _tokenService.GerarToken(usuario);

            usuario.Senha = null;

            return Ok(new
            {
                Usuario = usuario,
                Token = token
            });
        }
    }
}

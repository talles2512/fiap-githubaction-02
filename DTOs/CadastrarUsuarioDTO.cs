﻿using FiapStore.Enum;

namespace FiapStore.DTOs
{
    public class CadastrarUsuarioDTO
    {
        public string Nome { get; set; }
        public string NomeUsuario { get; set; }
        public string Senha { get; set; }
        public TipoPermissao Permissao { get; set; }
    }
}

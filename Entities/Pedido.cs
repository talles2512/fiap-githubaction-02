using System.Text.Json.Serialization;

namespace FiapStore.Entities
{
    public class Pedido : Entidade
    {
        public string NomeProduto { get; set; }
        public decimal PrecoTotal { get; set; }
        public int UsuarioId { get; set; }

        [JsonIgnore]
        public Usuario Usuario { get; set;}

        public Pedido()
        {

        }

        public Pedido(Pedido pedido)
        {
            Id = pedido.Id;
            NomeProduto = pedido.NomeProduto;
            PrecoTotal = pedido.PrecoTotal;
            UsuarioId = pedido.UsuarioId;
        }
    }
}

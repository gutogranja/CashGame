using CashGame.Domain.Entities.Mensagens;
using Dapper.Contrib.Extensions;

namespace CashGame.Domain.Entities
{
    [Table("Clientes")]
    public class Cliente : Entity
    {
        private Cliente() : base("")
        {
        }

        public string Nome { get; set; }
        public string Telefone { get; set; }
        
        public Cliente(string nome, string fone, string usuarioCadastro) : base(usuarioCadastro)
        {
            Nome = nome;
            Telefone = fone;
            if (string.IsNullOrEmpty(nome))
                AdicionarNotificacao("Cliente", ClienteMensagem.Nome);
        }

        public void AlterarTelefone(string fone)
        {
            Telefone = fone;
        }
    }
}

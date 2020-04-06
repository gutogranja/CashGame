using CashGame.Domain.Entities.Mensagens;
using Dapper.Contrib.Extensions;

namespace CashGame.Domain.Entities
{
    [Table("Dealers")]
    public class Dealer : Entity
    {
        private Dealer() : base("")
        {
        }

        public string Nome { get; set; }
        public string Telefone { get; set; }

        public Dealer(string nome, string fone, string usuarioCadastro) : base(usuarioCadastro)
        {
            Nome = nome;
            Telefone = fone;
            if (string.IsNullOrEmpty(nome))
                AdicionarNotificacao("Dealer", DealerMensagem.Nome);
        }

        public void AlterarTelefone(string fone)
        {
            Telefone = fone;
        }
    }
}

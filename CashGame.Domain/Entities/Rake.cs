using CashGame.Domain.Entities.Mensagens;
using Dapper.Contrib.Extensions;
using System;

namespace CashGame.Domain.Entities
{
    [Table("Rake")]
    public class Rake : Entity
    {
        private Rake() : base("")
        {
        }

        public DateTime DataRetirada { get; set; }
        public double Valor { get; set; }

        public Rake(DateTime data, double valor, string usuarioCadastro) : base(usuarioCadastro)
        {
            DataRetirada = data;
            Valor = valor;
            if (data == null)
                AdicionarNotificacao("Rake", RakeMensagem.DataRetirada);
            if (valor <= 0)
                AdicionarNotificacao("Rake", RakeMensagem.Valor);
        }

        public void AlterarDataRetirada(DateTime data)
        {
            if (data == null)
                AdicionarNotificacao("Rake", RakeMensagem.DataRetirada);
            else
                DataRetirada = data;
        }

        public void AlterarValor(double valor)
        {
            if (valor <= 0)
                AdicionarNotificacao("Rake", RakeMensagem.Valor);
            else
                Valor = valor;
        }
    }
}

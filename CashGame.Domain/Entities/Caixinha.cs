using System;
using CashGame.Domain.Entities.Mensagens;
using Dapper.Contrib.Extensions;

namespace CashGame.Domain.Entities
{
    [Table("Caixinhas")]
    public class Caixinha : Entity
    {
        private Caixinha() : base("")
        {
        }

        public int IdDealer { get; set; }
        public DateTime Data { get; set; }
        public double Valor { get; set; }

        public Caixinha(int idDealer, DateTime data, double valor, string usuarioCadastro) : base(usuarioCadastro)
        {
            IdDealer = idDealer;
            Data = data;
            Valor = valor;
            if (idDealer <= 0)
                AdicionarNotificacao("Caixinha", CaixinhaMensagem.Dealer);
            if (data == null)
                AdicionarNotificacao("Caixinha", CaixinhaMensagem.Data);
            if(valor <=0)
                AdicionarNotificacao("Caixinha", CaixinhaMensagem.Valor);
        }

        public void AlterarValor(double valor)
        {
            if (valor <= 0)
                AdicionarNotificacao("Caixinha", CaixinhaMensagem.Valor);
            else
                Valor = valor;
        }
    }
}

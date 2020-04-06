using System;
using CashGame.Domain.Entities.Mensagens;
using Dapper.Contrib.Extensions;

namespace CashGame.Domain.Entities
{
    [Table("ComprarFichas")]
    public class ComprarFicha : Entity
    {
        private ComprarFicha() : base("")
        {
        }

        public int IdCliente { get; set; }
        public DateTime Data { get; set; }
        public double Valor { get; set; }

        public ComprarFicha(int idCli, DateTime data, double valor, string usuarioCadastro) : base(usuarioCadastro)
        {
            IdCliente = idCli;
            Data = data;
            Valor = valor;
            if (idCli <= 0)
                AdicionarNotificacao("ComprarFichas", ComprarFichaMensagem.Cliente);
            if (data == null)
                AdicionarNotificacao("ComprarFichas", ComprarFichaMensagem.Data);
            if (valor <= 0)
                AdicionarNotificacao("ComprarFichas", ComprarFichaMensagem.Valor);
        }

        public void AlterarValor(double valor)
        {
            if (valor <= 0)
                AdicionarNotificacao("ComprarFichas", ComprarFichaMensagem.Valor);
            else
                Valor = valor;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using CashGame.Domain.Entities.Mensagens;
using Dapper.Contrib.Extensions;

namespace CashGame.Domain.Entities
{
    [Table("PagtoJogador")]
    public class PagtoJogador : Entity
    {
        private PagtoJogador() : base("")
        {
        }

        public int IdCliente { get; set; }
        public DateTime Data { get; set; }
        public double Valor { get; set; }

        public PagtoJogador(int idCli, DateTime data, double valor, string usuarioCadastro) : base(usuarioCadastro)
        {
            IdCliente = idCli;
            Data = data;
            Valor = valor;
            if (idCli <= 0)
                AdicionarNotificacao("PagtoJogador", PagtoJogadorMensagem.Cliente);
            if (data == null)
                AdicionarNotificacao("PagtoJogador", PagtoJogadorMensagem.Data);
            if (valor <= 0)
                AdicionarNotificacao("PagtoJogador", PagtoJogadorMensagem.Valor);
        }

        //public void AlterarCliente(int idCli)
        //{
        //    if (idCli <= 0)
        //        AdicionarNotificacao("PagtoJogador", PagtoJogadorMensagem.Cliente);
        //    else
        //        IdCliente = idCli;
        //}

        //public void AlterarData(DateTime data)
        //{
        //    if (data == null)
        //        AdicionarNotificacao("PagtoJogador", PagtoJogadorMensagem.Data);
        //    else
        //        Data = data;
        //}

        public void AlterarValor(double valor)
        {
            if (valor <= 0)
                AdicionarNotificacao("PagtoJogador", PagtoJogadorMensagem.Valor);
            else
                Valor = valor;
        }
    }
}

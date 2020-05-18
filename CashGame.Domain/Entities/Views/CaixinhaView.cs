using System;
using System.Collections.Generic;
using System.Text;

namespace CashGame.Domain.Entities.Views
{
    public class CaixinhaView
    {
        public int Id { get; set; }
        public int IdDealer { get; set; }
        public DateTime Data { get; set; } = DateTime.Now;
        public double Valor { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace CashGame.Domain.Entities.Views
{
    public class CaixinhaView
    {
        public int Id { get; set; }
        public int IdDealer { get; set; }
        public DateTime Data { get; set; }
        public double Valor { get; set; }
    }
}

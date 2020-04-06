using System;

namespace CashGame.Domain.Entities.Views
{
    public class RakeView
    {
        public int Id { get; set; }
        public DateTime DataRetirada { get; set; }
        public double Valor { get; set; }
    }
}

using System;

namespace CashGame.Domain.Entities.Views
{
    public class ComprarFichaView
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public DateTime Data { get; set; } = DateTime.Now;
        public double Valor { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
    }
}

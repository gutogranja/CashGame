using System;

namespace CashGame.Domain.Entities.Requests
{
    public class ComprarFichaRequest
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public DateTime Data { get; set; }
        public double Valor { get; set; }
    }
}

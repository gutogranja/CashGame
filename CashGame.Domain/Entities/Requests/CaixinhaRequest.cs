using System;

namespace CashGame.Domain.Entities.Requests
{
    public class CaixinhaRequest
    {
        public int Id { get; set; }
        public int IdDealer { get; set; }
        public DateTime Data { get; set; }
        public double Valor { get; set; }
    }
}

using System;

namespace CashGame.Domain.Entities.Requests
{
    public class RakeRequest
    {
        public int Id { get; set; }
        public DateTime DataRetirada { get; set; }
        public double Valor{ get; set; }
    }
}

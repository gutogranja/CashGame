using System;
using System.Collections.Generic;
using System.Text;

namespace CashGame.Domain.Entities.Requests
{
    public class DealerRequest
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace CashGame.Domain.Helpers
{
    public class Notification
    {
        public string Chave { get; set; }
        public string Mensagem { get; set; }

        public Notification(string key, string msg)
        {
            Chave = key;
            Mensagem = msg;
        }
    }
}

﻿using System;

namespace CashGame.Domain.Entities.Views
{
    public class PagtoJogadorView
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public DateTime Data { get; set; } = DateTime.Now;
        public double Valor { get; set; }
        public string Nome { get; set; }
    }
}

﻿using System;

namespace CashGame.Domain.Entities.Views
{
    public class PagtoJogadorView
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public DateTime Data { get; set; }
        public double Valor { get; set; }
    }
}

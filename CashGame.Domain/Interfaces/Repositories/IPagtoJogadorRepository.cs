using CashGame.Domain.Entities;
using CashGame.Domain.Entities.Views;
using System;
using System.Collections.Generic;

namespace CashGame.Domain.Interfaces.Repositories
{
    public interface IPagtoJogadorRepository : IRepositoryBase<PagtoJogador, PagtoJogadorView>
    {
        bool VerificarPagamentoJogador(int idPagto);
        List<PagtoJogadorView> PagtoJogadorFechamento(DateTime inicial, DateTime final);
        IEnumerable<PagtoJogadorView> ListarPorData(DateTime data);
    }
}

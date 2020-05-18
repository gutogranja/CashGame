using CashGame.Domain.Entities.Views;
using System.Collections.Generic;

namespace CashGame.Domain.Interfaces.Repositories
{
    public interface IFechamentoRepository
    {
        void ExecutarFechamento(List<ComprarFichaView> lstComprarFichas, List<PagtoJogadorView> lstPagtoJogador, List<CaixinhaView> lstCaixinha, List<RakeView> lstRake);
    }
}

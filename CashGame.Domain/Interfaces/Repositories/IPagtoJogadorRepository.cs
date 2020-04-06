using CashGame.Domain.Entities;
using CashGame.Domain.Entities.Views;

namespace CashGame.Domain.Interfaces.Repositories
{
    public interface IPagtoJogadorRepository : IRepositoryBase<PagtoJogador, PagtoJogadorView>
    {
        bool VerificarPagamentoJogador(int idPagto);
    }
}

using CashGame.Domain.Entities;
using CashGame.Domain.Entities.Views;

namespace CashGame.Domain.Interfaces.Repositories
{
    public interface IComprarFichaRepository : IRepositoryBase<ComprarFicha, ComprarFichaView>
    {
        bool VerificarCompraFichas(int idCompra);
    }
}

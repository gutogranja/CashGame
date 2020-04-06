using CashGame.Domain.Entities;
using CashGame.Domain.Entities.Views;

namespace CashGame.Domain.Interfaces.Repositories
{
    public interface IDealerRepository : IRepositoryBase<Dealer, DealerView>
    {
        bool VerificarDealerExistente(string nome);
    }
}

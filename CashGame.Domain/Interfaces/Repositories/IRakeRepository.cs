using CashGame.Domain.Entities;
using CashGame.Domain.Entities.Views;

namespace CashGame.Domain.Interfaces.Repositories
{
    public interface IRakeRepository : IRepositoryBase<Rake,RakeView> 
    {
        bool VerificarRakeRetirado(int idRake);
    }
}

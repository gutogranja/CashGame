using CashGame.Domain.Entities;
using CashGame.Domain.Entities.Views;

namespace CashGame.Domain.Interfaces.Repositories
{
    public interface IClienteRepository : IRepositoryBase<Cliente,ClienteView>
    {
        bool VerificarClienteExistente(string nome);
    }
}

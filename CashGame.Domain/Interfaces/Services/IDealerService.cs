using CashGame.Domain.Entities;
using CashGame.Domain.Entities.Requests;
using CashGame.Domain.Entities.Views;
using System.Collections.Generic;

namespace CashGame.Domain.Interfaces.Services
{
    public interface IDealerService : INotificationService
    {
        IEnumerable<DealerView> ListarTodos();
        Dealer ObterPorId(int id);
        Dealer Incluir(DealerRequest dealer, string usuarioCadastro);
        Dealer Alterar(DealerRequest dealer, string usuarioCadastro);
        void Inativar(int id, string usuarioCadastro);
    }
}

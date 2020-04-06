using CashGame.Domain.Entities;
using CashGame.Domain.Entities.Requests;
using CashGame.Domain.Entities.Views;
using System.Collections.Generic;

namespace CashGame.Domain.Interfaces.Services
{
    public interface IRakeService : INotificationService
    {
        IEnumerable<RakeView> ListarTodos();
        Rake ObterPorId(int id);
        Rake Incluir(RakeRequest rake, string usuarioCadastro);
        Rake Alterar(RakeRequest rake, string usuarioCadastro);
        void Inativar(int id, string usuarioCadastro);
    }
}

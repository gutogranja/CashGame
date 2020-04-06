using CashGame.Domain.Entities;
using CashGame.Domain.Entities.Requests;
using CashGame.Domain.Entities.Views;
using System.Collections.Generic;

namespace CashGame.Domain.Interfaces.Services
{
    public interface ICaixinhaService : INotificationService
    {
        IEnumerable<CaixinhaView> ListarTodos();
        Caixinha ObterPorId(int id);
        Caixinha Incluir(CaixinhaRequest caixinha, string usuarioCadastro);
        Caixinha Alterar(CaixinhaRequest caixinha, string usuarioCadastro);
        void Inativar(int id, string usuarioCadastro);
    }
}

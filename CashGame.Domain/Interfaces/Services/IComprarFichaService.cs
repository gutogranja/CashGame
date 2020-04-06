using CashGame.Domain.Entities;
using CashGame.Domain.Entities.Requests;
using CashGame.Domain.Entities.Views;
using System.Collections.Generic;

namespace CashGame.Domain.Interfaces.Services
{
    public interface IComprarFichaService : INotificationService
    {
        IEnumerable<ComprarFichaView> ListarTodos();
        ComprarFicha ObterPorId(int id);
        ComprarFicha Incluir(ComprarFichaRequest ficha, string usuarioCadastro);
        ComprarFicha Alterar(ComprarFichaRequest ficha, string usuarioCadastro);
        void Inativar(int id, string usuarioCadastro);
    }
}

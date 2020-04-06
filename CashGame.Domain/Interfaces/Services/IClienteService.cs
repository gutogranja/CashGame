using System.Collections.Generic;
using CashGame.Domain.Entities;
using CashGame.Domain.Entities.Requests;
using CashGame.Domain.Entities.Views;

namespace CashGame.Domain.Interfaces.Services
{
    public interface IClienteService : INotificationService
    {
        IEnumerable<ClienteView> ListarTodos();
        Cliente ObterPorId(int id);
        Cliente Incluir(ClienteRequest cliente, string usuarioCadastro);
        Cliente Alterar(ClienteRequest cliente, string usuarioCadastro);
        void Inativar(int id, string usuarioCadastro);
    }
}

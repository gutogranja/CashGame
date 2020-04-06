using CashGame.Domain.Entities;
using CashGame.Domain.Entities.Requests;
using CashGame.Domain.Entities.Views;
using System.Collections.Generic;

namespace CashGame.Domain.Interfaces.Services
{
    public interface IPagtoJogadorService : INotificationService
    {
        IEnumerable<PagtoJogadorView> ListarTodos();
        PagtoJogador ObterPorId(int id);
        PagtoJogador Incluir(PagtoJogadorRequest pagto, string usuarioCadastro);
        PagtoJogador Alterar(PagtoJogadorRequest pagto, string usuarioCadastro);
        void Inativar(int id, string usuarioCadastro);
    }
}

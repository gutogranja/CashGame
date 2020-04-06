using CashGame.Domain.Entities;
using System.Collections.Generic;

namespace CashGame.Domain.Interfaces.Repositories
{
    public interface IRepositoryBase<T,TView> where T : Entity where TView : class
    {
        IEnumerable<TView> ListarTodos();
        T ObterPorId(int id);
        T Incluir(T entity);
        T Alterar(T entity);
    }
}

using CashGame.Domain.Entities;
using CashGame.Domain.Entities.Views;
using System;

namespace CashGame.Domain.Interfaces.Repositories
{
    public interface ICaixinhaRepository : IRepositoryBase<Caixinha, CaixinhaView>
    {
        bool VerificarCaixinhaParaDealer(int idDealer, DateTime data);
        //bool ListarTodasCaixinhasPorDia(DateTime data);
    }
}

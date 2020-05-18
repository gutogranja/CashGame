using CashGame.Domain.Entities;
using CashGame.Domain.Entities.Views;
using System;
using System.Collections.Generic;

namespace CashGame.Domain.Interfaces.Repositories
{
    public interface ICaixinhaRepository : IRepositoryBase<Caixinha, CaixinhaView>
    {
        bool VerificarCaixinhaParaDealer(int idDealer, DateTime data);
        List<CaixinhaView> CaixinhaFechamento(DateTime inicial, DateTime final);
        IEnumerable<CaixinhaView> ListarPorData(DateTime data);
    }
}

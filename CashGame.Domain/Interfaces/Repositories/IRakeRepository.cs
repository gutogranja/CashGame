using CashGame.Domain.Entities;
using CashGame.Domain.Entities.Views;
using System;
using System.Collections.Generic;

namespace CashGame.Domain.Interfaces.Repositories
{
    public interface IRakeRepository : IRepositoryBase<Rake,RakeView> 
    {
        bool VerificarRakeRetirado(int idRake);
        List<RakeView> RakeFechamento(DateTime inicial, DateTime final);
        IEnumerable<RakeView> ListarPorData(DateTime data);
    }
}

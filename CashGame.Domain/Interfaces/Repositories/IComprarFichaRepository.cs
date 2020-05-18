using CashGame.Domain.Entities;
using CashGame.Domain.Entities.Views;
using System;
using System.Collections.Generic;

namespace CashGame.Domain.Interfaces.Repositories
{
    public interface IComprarFichaRepository : IRepositoryBase<ComprarFicha, ComprarFichaView>
    {
        bool VerificarCompraFichas(int idCompra);
        List<ComprarFichaView> ComprarFichaFechamento(DateTime inicial, DateTime final);
        IEnumerable<ComprarFichaView> ListarPorData(DateTime data);
    }
}

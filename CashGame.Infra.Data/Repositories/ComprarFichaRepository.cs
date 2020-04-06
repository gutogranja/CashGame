using CashGame.Domain.Entities;
using CashGame.Domain.Entities.Views;
using CashGame.Domain.Interfaces.Repositories;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace CashGame.Infra.Data.Repositories
{
    public class ComprarFichaRepository : RepositoryBase<ComprarFicha, ComprarFichaView>, IComprarFichaRepository
    {
        public override IEnumerable<ComprarFichaView> ListarTodos()
        {
            return cn.Query<ComprarFichaView>("SELECT * FROM ComprarFichas WHERE Ativo = 1 ORDER BY Data,IdCliente").ToList();
        }

        public bool VerificarCompraFichas(int idCompra)
        {
            return cn.Query<int>($"SELECT TOP 1 1 FROM ComprarFichas WHERE Id = {idCompra} AND Ativo = 1").Any();
        }
    }
}

using CashGame.Domain.Entities;
using CashGame.Domain.Entities.Views;
using CashGame.Domain.Interfaces.Repositories;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace CashGame.Infra.Data.Repositories
{
    public class RakeRepository : RepositoryBase<Rake, RakeView>, IRakeRepository
    {
        public override IEnumerable<RakeView> ListarTodos()
        {
            return cn.Query<RakeView>("SELECT * FROM Rake WHERE Ativo = 1 ORDER BY DataRetirada").ToList();
        }

        public bool VerificarRakeRetirado(int idRake)
        {
            return cn.Query<int>($"SELECT TOP 1 1 FROM Rake WHERE Id = {idRake} AND Ativo = 1").Any();
        }
    }
}

using CashGame.Domain.Entities;
using CashGame.Domain.Entities.Views;
using CashGame.Domain.Interfaces.Repositories;
using Dapper;
using System;
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

        public List<RakeView> RakeFechamento(DateTime inicial,DateTime final)
        {
            return cn.Query<RakeView>($"SELECT CAST(DataRetirada AS DATE) AS DataRetirada,SUM(Valor) AS Valor FROM Rake WHERE CAST(DataRetirada AS DATE) BETWEEN CAST('{inicial}' AS DATE) AND CAST('{final}' AS DATE) AND Ativo = 1 GROUP BY CAST(DataRetirada AS DATE)").ToList();
        }

        public IEnumerable<RakeView> ListarPorData(DateTime data)
        {
            return cn.Query<RakeView>($"SELECT * FROM Rake WHERE CAST(DataRetirada AS DATE) = CAST('{data}' AS DATE) AND Ativo = 1").ToList();
        }
    }
}

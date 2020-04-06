using CashGame.Domain.Entities;
using CashGame.Domain.Entities.Views;
using CashGame.Domain.Interfaces.Repositories;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace CashGame.Infra.Data.Repositories
{
    public class DealerRepository : RepositoryBase<Dealer, DealerView>, IDealerRepository
    {
        public override IEnumerable<DealerView> ListarTodos()
        {
            return cn.Query<DealerView>("SELECT * FROM Dealers WHERE Ativo = 1 ORDER BY Nome").ToList();
        }

        public bool VerificarDealerExistente(string nomeDealer)
        {
            return cn.Query<int>($"SELECT TOP 1 1 FROM Dealers WHERE Nome = '{nomeDealer}' AND Ativo = 1").Any();
        }
    }
}

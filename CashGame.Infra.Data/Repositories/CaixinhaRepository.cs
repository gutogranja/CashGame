using CashGame.Domain.Entities;
using CashGame.Domain.Entities.Views;
using CashGame.Domain.Interfaces.Repositories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CashGame.Infra.Data.Repositories
{
    public class CaixinhaRepository : RepositoryBase<Caixinha, CaixinhaView>, ICaixinhaRepository
    {
        public override IEnumerable<CaixinhaView> ListarTodos()
        {
            return cn.Query<CaixinhaView>("SELECT * FROM Caixinhas WHERE Ativo = 1 ORDER BY Data").ToList();
        }

        public bool VerificarCaixinhaParaDealer(int idDealer,DateTime data)
        {
            return cn.Query<int>($"SELECT TOP 1 1 FROM Caixinhas WHERE IdDealer = {idDealer} AND Data = '{data}' AND Ativo = 1").Any();
        }
    }
}

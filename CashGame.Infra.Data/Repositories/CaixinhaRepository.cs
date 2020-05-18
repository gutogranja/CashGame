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
            return cn.Query<CaixinhaView>("SELECT DEA.Nome,DEA.Telefone,CX.* FROM Caixinhas AS CX INNER JOIN Dealers AS DEA ON CX.IdDealer = DEA.Id WHERE CX.Ativo = 1 ORDER BY CX.Data").ToList();
        }

        public bool VerificarCaixinhaParaDealer(int idDealer,DateTime data)
        {
            return cn.Query<int>($"SELECT TOP 1 1 FROM Caixinhas WHERE IdDealer = {idDealer} AND Data = '{data}' AND Ativo = 1").Any();
        }

        public List<CaixinhaView> CaixinhaFechamento(DateTime inicial,DateTime final)
        {
            return cn.Query<CaixinhaView>($"SELECT CX.Id,CX.IdDealer,DEA.Nome,CX.Data,CX.Valor FROM Caixinhas AS CX INNER JOIN Dealers AS DEA ON CX.IdDealer = DEA.Id WHERE CAST(CX.Data AS DATE) BETWEEN CAST('{inicial}' AS DATE) AND CAST('{final}' AS DATE) AND CX.Ativo = 1 ORDER BY CX.Data").ToList();
        }

        public IEnumerable<CaixinhaView> ListarPorData(DateTime data)
        {
            return cn.Query<CaixinhaView>($"SELECT DEA.Nome,DEA.Telefone,CX.* FROM Caixinhas AS CX INNER JOIN Dealers AS DEA ON CX.IdDealer = DEA.Id WHERE CAST(CX.Data AS DATE) = CAST('{data}' AS DATE) AND CX.Ativo = 1").ToList();
        }
    }
}

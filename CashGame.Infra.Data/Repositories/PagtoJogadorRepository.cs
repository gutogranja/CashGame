using CashGame.Domain.Entities;
using CashGame.Domain.Entities.Views;
using CashGame.Domain.Interfaces.Repositories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CashGame.Infra.Data.Repositories
{
    public class PagtoJogadorRepository : RepositoryBase<PagtoJogador, PagtoJogadorView>, IPagtoJogadorRepository
    {
        public override IEnumerable<PagtoJogadorView> ListarTodos()
        {
            return cn.Query<PagtoJogadorView>("SELECT CLI.Nome,PG.* FROM PagtoJogador AS PG INNER JOIN Clientes AS CLI ON PG.IdCliente = CLI.Id WHERE PG.Ativo = 1 ORDER BY PG.Data").ToList();
        }

        public bool VerificarPagamentoJogador(int idPagto)
        {
            return cn.Query<int>($"SELECT TOP 1 1 FROM PagtoJogador WHERE Id = {idPagto} AND Ativo = 1").Any();
        }

        public List<PagtoJogadorView> PagtoJogadorFechamento(DateTime inicial,DateTime final)
        {
            return cn.Query<PagtoJogadorView>($"SELECT CAST(Data AS DATE) AS Data,SUM(Valor) AS Valor FROM PagtoJogador WHERE CAST(Data AS DATE) BETWEEN CAST('{inicial}' AS DATE) AND CAST('{final}' AS DATE) AND Ativo = 1 GROUP BY CAST(Data AS DATE)").ToList();
        }

        public IEnumerable<PagtoJogadorView> ListarPorData(DateTime data)
        {
            return cn.Query<PagtoJogadorView>($"SELECT CLI.Nome,PG.* FROM PagtoJogador AS PG INNER JOIN Clientes AS CLI ON PG.IdCliente = CLI.Id WHERE CAST(PG.Data AS DATE) = CAST('{data}' AS DATE) AND PG.Ativo = 1").ToList();
        }
    }
}

using CashGame.Domain.Entities;
using CashGame.Domain.Entities.Views;
using CashGame.Domain.Interfaces.Repositories;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace CashGame.Infra.Data.Repositories
{
    public class PagtoJogadorRepository : RepositoryBase<PagtoJogador, PagtoJogadorView>, IPagtoJogadorRepository
    {
        public override IEnumerable<PagtoJogadorView> ListarTodos()
        {
            return cn.Query<PagtoJogadorView>("SELECT * FROM PagtoJogador WHERE Ativo = 1 ORDER BY Data,IdCliente").ToList();
        }

        public bool VerificarPagamentoJogador(int idPagto)
        {
            return cn.Query<int>($"SELECT TOP 1 1 FROM PagtoJogador WHERE Id = {idPagto} AND Ativo = 1").Any();
        }
    }
}

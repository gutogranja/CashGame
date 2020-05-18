using CashGame.Domain.Entities;
using CashGame.Domain.Entities.Views;
using CashGame.Domain.Interfaces.Repositories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CashGame.Infra.Data.Repositories
{
    public class ComprarFichaRepository : RepositoryBase<ComprarFicha, ComprarFichaView>, IComprarFichaRepository
    {
        public override IEnumerable<ComprarFichaView> ListarTodos()
        {
            return cn.Query<ComprarFichaView>("SELECT CLI.Nome,CLI.Telefone,COM.* FROM ComprarFichas AS COM INNER JOIN Clientes AS CLI ON COM.IdCliente = CLI.Id WHERE COM.Ativo = 1 ORDER BY COM.Data").ToList();
        }

        public bool VerificarCompraFichas(int idCompra)
        {
            return cn.Query<int>($"SELECT TOP 1 1 FROM ComprarFichas WHERE Id = {idCompra} AND Ativo = 1").Any();
        }

        public List<ComprarFichaView> ComprarFichaFechamento(DateTime inicial,DateTime final)
        {
            return cn.Query<ComprarFichaView>($"SELECT CAST(Data AS DATE) AS Data,SUM(Valor) AS Valor FROM ComprarFichas WHERE CAST(Data AS DATE) BETWEEN CAST('{inicial}' AS DATE) AND CAST('{final}' AS DATE) AND Ativo = 1 GROUP BY CAST(Data AS DATE)").ToList();
        }

        public IEnumerable<ComprarFichaView> ListarPorData(DateTime data)
        {
            return cn.Query<ComprarFichaView>($"SELECT CLI.Nome,CLI.Telefone,COM.* FROM ComprarFichas AS COM INNER JOIN Clientes AS CLI ON COM.IdCliente = CLI.Id WHERE CAST(COM.Data AS DATE) = CAST('{data}' AS DATE) AND COM.Ativo = 1").ToList();
        }
    }
}

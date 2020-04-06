using CashGame.Domain.Entities;
using CashGame.Domain.Entities.Views;
using CashGame.Domain.Interfaces.Repositories;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace CashGame.Infra.Data.Repositories
{
    public class ClienteRepository : RepositoryBase<Cliente,ClienteView> , IClienteRepository
    {
        public override IEnumerable<ClienteView> ListarTodos()
        {
            return cn.Query<ClienteView>("SELECT * FROM Clientes WHERE Ativo = 1 ORDER BY Nome").ToList();
        }

        public bool VerificarClienteExistente(string nomeCliente)
        {
            return cn.Query<int>($"SELECT TOP 1 1 FROM Clientes WHERE Nome = '{nomeCliente}' AND Ativo = 1").Any();
        }
    }
}

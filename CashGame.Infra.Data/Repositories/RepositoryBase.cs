﻿using CashGame.Domain.Entities;
using CashGame.Domain.Interfaces.Repositories;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CashGame.Infra.Data.Repositories
{
    public abstract class RepositoryBase<T, TView> : IRepositoryBase<T, TView> where T : Entity where TView : class
    {
        protected SqlConnection cn = new SqlConnection("");
        public abstract IEnumerable<TView> ListarTodos();

        public T ObterPorId(int id)
        {
            return cn.Get<T>(id);
        }

        public T Incluir(T entity)
        {
            var retorno = cn.Insert(entity);
            if (retorno > 0)
                return entity;
            return null;
        }

        public T Alterar(T entity)
        {
            bool retorno = cn.Update(entity);
            if (retorno)
                return entity;
            return null;
        }
    }
}

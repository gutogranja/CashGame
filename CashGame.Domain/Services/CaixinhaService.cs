using CashGame.Domain.Entities;
using CashGame.Domain.Entities.Mensagens;
using CashGame.Domain.Entities.Requests;
using CashGame.Domain.Entities.Views;
using CashGame.Domain.Interfaces.Repositories;
using CashGame.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace CashGame.Domain.Services
{
    public class CaixinhaService : BaseService, ICaixinhaService
    {
        private readonly ICaixinhaRepository repositorio;

        public CaixinhaService(ICaixinhaRepository repositorio)
        {
            this.repositorio = repositorio;
        }

        public IEnumerable<CaixinhaView> ListarTodos()
        {
            return repositorio.ListarTodos();
        }

        public Caixinha ObterPorId(int id)
        {
            return repositorio.ObterPorId(id);
        }

        public Caixinha Incluir(CaixinhaRequest request, string usuarioCadastro)
        {
            var novaCaixinha = new Caixinha(request.IdDealer, request.Data, request.Valor, usuarioCadastro);
            ValidarCaixinha(novaCaixinha);
            if (Validar)
            {
                bool caixinhaDada = repositorio.VerificarCaixinhaParaDealer(request.IdDealer, request.Data);
                if (!caixinhaDada)
                {
                    return repositorio.Incluir(novaCaixinha);
                }
                else
                    AdicionarNotificacao("Caixinha", CaixinhaMensagem.CxDada);
            }
            return null;
        }

        public Caixinha Alterar(CaixinhaRequest request, string usuarioCadastro)
        {
            var caixinhaDada = repositorio.ObterPorId(request.Id);
            if (caixinhaDada != null)
            {
                caixinhaDada.AlterarValor(request.Valor);
                ValidarCaixinha(caixinhaDada);
                if (Validar)
                {
                    return repositorio.Alterar(caixinhaDada);
                }
            }
            else
                AdicionarNotificacao("Caixinha", CaixinhaMensagem.CxNaoDada);
            return null;
        }

        public void Inativar(int id, string usuario)
        {
            if (id <= 0)
                AdicionarNotificacao("Caixinha", CaixinhaMensagem.CxNaoDada);
            var caixinhaDada = repositorio.ObterPorId(id);
            if (caixinhaDada != null)
            {
                caixinhaDada.Inativar(usuario);
                repositorio.Alterar(caixinhaDada);
            }
            else
                AdicionarNotificacao("Caixinha", CaixinhaMensagem.Inativar);
        }

        public IEnumerable<CaixinhaView> ListarPorData(DateTime data)
        {
            return repositorio.ListarPorData(data);
        }

        private void ValidarCaixinha(Caixinha caixinha)
        {
            if (!caixinha.Validar)
                AdicionarNotificacao(caixinha.Notificacoes);
        }
    }
}

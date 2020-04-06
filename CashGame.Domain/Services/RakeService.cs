using CashGame.Domain.Entities;
using CashGame.Domain.Entities.Mensagens;
using CashGame.Domain.Entities.Requests;
using CashGame.Domain.Entities.Views;
using CashGame.Domain.Interfaces.Repositories;
using CashGame.Domain.Interfaces.Services;
using System.Collections.Generic;

namespace CashGame.Domain.Services
{
    public class RakeService : BaseService, IRakeService
    {
        private readonly IRakeRepository repositorio;

        public RakeService(IRakeRepository repositorio)
        {
            this.repositorio = repositorio;
        }

        public IEnumerable<RakeView> ListarTodos()
        {
            return repositorio.ListarTodos();
        }

        public Rake ObterPorId(int id)
        {
            return repositorio.ObterPorId(id);
        }

        public Rake Incluir(RakeRequest request, string usuarioCadastro)
        {
            var novoRake = new Rake(request.DataRetirada, request.Valor, usuarioCadastro);
            ValidarRake(novoRake);
            if (Validar)
            {
                bool rakeRetirado= repositorio.VerificarRakeRetirado(request.Id);
                if (!rakeRetirado)
                {
                    return repositorio.Incluir(novoRake);
                }
                else
                    AdicionarNotificacao("Rake", RakeMensagem.Retirado);
            }
            return null;
        }

        public Rake Alterar(RakeRequest request, string usuarioCadastro)
        {
            var rakeRetirado= repositorio.ObterPorId(request.Id);
            if (rakeRetirado != null)
            {
                rakeRetirado.AlterarDataRetirada(request.DataRetirada);
                rakeRetirado.AlterarValor(request.Valor);
                ValidarRake(rakeRetirado);
                if (Validar)
                {
                    return repositorio.Alterar(rakeRetirado);
                }
            }
            else
                AdicionarNotificacao("Rake", RakeMensagem.NaoEncontrado);
            return null;
        }

        public void Inativar(int id, string usuario)
        {
            if (id <= 0)
                AdicionarNotificacao("Rake", RakeMensagem.NaoEncontrado);
            var rakeRetirado= repositorio.ObterPorId(id);
            if (rakeRetirado != null)
            {
                rakeRetirado.Inativar(usuario);
                repositorio.Alterar(rakeRetirado);
            }
            else
                AdicionarNotificacao("Rake", RakeMensagem.Inativar);
        }

        private void ValidarRake(Rake rake)
        {
            if (!rake.Validar)
                AdicionarNotificacao(rake.Notificacoes);
        }
    }
}

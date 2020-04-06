using CashGame.Domain.Entities;
using CashGame.Domain.Entities.Mensagens;
using CashGame.Domain.Entities.Requests;
using CashGame.Domain.Entities.Views;
using CashGame.Domain.Interfaces.Repositories;
using CashGame.Domain.Interfaces.Services;
using System.Collections.Generic;

namespace CashGame.Domain.Services
{
    public class DealerService : BaseService, IDealerService
    {
        private readonly IDealerRepository repositorio;

        public DealerService(IDealerRepository repositorio)
        {
            this.repositorio = repositorio;
        }

        public IEnumerable<DealerView> ListarTodos()
        {
            return repositorio.ListarTodos();
        }

        public Dealer ObterPorId(int id)
        {
            return repositorio.ObterPorId(id);
        }

        public Dealer Incluir(DealerRequest request, string usuarioCadastro)
        {
            var novoDealer = new Dealer(request.Nome, request.Telefone, usuarioCadastro);
            ValidarDealer(novoDealer);
            if (Validar)
            {
                bool dealerExistente = repositorio.VerificarDealerExistente(request.Nome);
                if (!dealerExistente)
                {
                    return repositorio.Incluir(novoDealer);
                }
                else
                    AdicionarNotificacao("Dealer", DealerMensagem.Cadastrado);
            }
            return null;
        }

        public Dealer Alterar(DealerRequest request, string usuarioCadastro)
        {
            var dealerExistente = repositorio.ObterPorId(request.Id);
            if (dealerExistente != null)
            {
                dealerExistente.AlterarTelefone(request.Telefone);
                ValidarDealer(dealerExistente);
                if (Validar)
                {
                    return repositorio.Alterar(dealerExistente);
                }
            }
            else
                AdicionarNotificacao("Dealer", DealerMensagem.NaoEncontrado);
            return null;
        }

        public void Inativar(int id, string usuario)
        {
            if (id <= 0)
                AdicionarNotificacao("Dealer", DealerMensagem.NaoEncontrado);
            var dealerExistente = repositorio.ObterPorId(id);
            if (dealerExistente != null)
            {
                dealerExistente.Inativar(usuario);
                repositorio.Alterar(dealerExistente);
            }
            else
                AdicionarNotificacao("Dealer", DealerMensagem.Inativar);
        }

        private void ValidarDealer(Dealer dealer)
        {
            if (!dealer.Validar)
                AdicionarNotificacao(dealer.Notificacoes);
        }
    }
}

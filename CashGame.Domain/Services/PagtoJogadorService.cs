using CashGame.Domain.Entities;
using CashGame.Domain.Entities.Mensagens;
using CashGame.Domain.Entities.Requests;
using CashGame.Domain.Entities.Views;
using CashGame.Domain.Interfaces.Repositories;
using CashGame.Domain.Interfaces.Services;
using System.Collections.Generic;

namespace CashGame.Domain.Services
{
    public class PagtoJogadorService : BaseService, IPagtoJogadorService
    {
        private readonly IPagtoJogadorRepository repositorio;

        public PagtoJogadorService(IPagtoJogadorRepository repositorio)
        {
            this.repositorio = repositorio;
        }

        public IEnumerable<PagtoJogadorView> ListarTodos()
        {
            return repositorio.ListarTodos();
        }

        public PagtoJogador ObterPorId(int id)
        {
            return repositorio.ObterPorId(id);
        }

        public PagtoJogador Incluir(PagtoJogadorRequest request, string usuarioCadastro)
        {
            var novoPagto = new PagtoJogador(request.IdCliente, request.Data, request.Valor, usuarioCadastro);
            ValidarPagto(novoPagto);
            if (Validar)
            {
                bool pagtoEfetuado = repositorio.VerificarPagamentoJogador(request.Id);
                if (!pagtoEfetuado)
                {
                    return repositorio.Incluir(novoPagto);
                }
                else
                    AdicionarNotificacao("PagtoJogador", PagtoJogadorMensagem.Efetuado);
            }
            return null;
        }

        public PagtoJogador Alterar(PagtoJogadorRequest request, string usuarioCadastro)
        {
            var pagtoEfetuado = repositorio.ObterPorId(request.Id);
            if (pagtoEfetuado != null)
            {
                pagtoEfetuado.AlterarValor(request.Valor);
                ValidarPagto(pagtoEfetuado);
                if (Validar)
                {
                    return repositorio.Alterar(pagtoEfetuado);
                }
            }
            else
                AdicionarNotificacao("PagtoJogador", PagtoJogadorMensagem.NaoEncontrado);
            return null;
        }

        public void Inativar(int id, string usuario)
        {
            if (id <= 0)
                AdicionarNotificacao("PagtoJogador", PagtoJogadorMensagem.NaoEncontrado);
            var pagtoEfetuado = repositorio.ObterPorId(id);
            if (pagtoEfetuado != null)
            {
                pagtoEfetuado.Inativar(usuario);
                repositorio.Alterar(pagtoEfetuado);
            }
            else
                AdicionarNotificacao("PagtoJogador", PagtoJogadorMensagem.Inativar);
        }

        private void ValidarPagto(PagtoJogador pagto)
        {
            if (!pagto.Validar)
                AdicionarNotificacao(pagto.Notificacoes);
        }
    }
}

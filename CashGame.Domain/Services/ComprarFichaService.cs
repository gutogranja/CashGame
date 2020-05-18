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
    public class ComprarFichaService : BaseService, IComprarFichaService
    {
        private readonly IComprarFichaRepository repositorio;

        public ComprarFichaService(IComprarFichaRepository repositorio)
        {
            this.repositorio = repositorio;
        }

        public IEnumerable<ComprarFichaView> ListarTodos()
        {
            return repositorio.ListarTodos();
        }

        public ComprarFicha ObterPorId(int id)
        {
            return repositorio.ObterPorId(id);
        }

        public ComprarFicha Incluir(ComprarFichaRequest request, string usuarioCadastro)
        {
            var novaCompra = new ComprarFicha(request.IdCliente, request.Data, request.Valor, usuarioCadastro);
            ValidarCompra(novaCompra);
            if (Validar)
            {
                bool compraEfetuada = repositorio.VerificarCompraFichas(request.Id);
                if (!compraEfetuada)
                {
                    return repositorio.Incluir(novaCompra);
                }
                else
                    AdicionarNotificacao("ComprarFichas", ComprarFichaMensagem.Efetuada);
            }
            return null;
        }

        public ComprarFicha Alterar(ComprarFichaRequest request, string usuarioCadastro)
        {
            var compraEfetuada = repositorio.ObterPorId(request.Id);
            if (compraEfetuada != null)
            {
                compraEfetuada.AlterarValor(request.Valor);
                ValidarCompra(compraEfetuada);
                if (Validar)
                {
                    return repositorio.Alterar(compraEfetuada);
                }
            }
            else
                AdicionarNotificacao("ComprarFichas", ComprarFichaMensagem.NaoEncontrado);
            return null;
        }

        public void Inativar(int id, string usuario)
        {
            if (id <= 0)
                AdicionarNotificacao("ComprarFichas", ComprarFichaMensagem.NaoEncontrado);
            var compraEfetuada = repositorio.ObterPorId(id);
            if (compraEfetuada != null)
            {
                compraEfetuada.Inativar(usuario);
                repositorio.Alterar(compraEfetuada);
            }
            else
                AdicionarNotificacao("ComprarFichas", ComprarFichaMensagem.Inativar);
        }

        public IEnumerable<ComprarFichaView> ListarPorData(DateTime data)
        {
            return repositorio.ListarPorData(data);
        }

        private void ValidarCompra(ComprarFicha compra)
        {
            if (!compra.Validar)
                AdicionarNotificacao(compra.Notificacoes);
        }
    }
}

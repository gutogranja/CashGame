using CashGame.Domain.Entities;
using CashGame.Domain.Entities.Mensagens;
using CashGame.Domain.Entities.Requests;
using CashGame.Domain.Entities.Views;
using CashGame.Domain.Interfaces.Repositories;
using CashGame.Domain.Interfaces.Services;
using System.Collections.Generic;

namespace CashGame.Domain.Services
{
    public class ClienteService : BaseService, IClienteService
    {
        private readonly IClienteRepository repositorio;

        public ClienteService(IClienteRepository repositorio)
        {
            this.repositorio = repositorio;
        }

        public IEnumerable<ClienteView> ListarTodos()
        {
            return repositorio.ListarTodos();
        }

        public Cliente ObterPorId(int id)
        {
            return repositorio.ObterPorId(id);
        }

        public Cliente Incluir(ClienteRequest request, string usuarioCadastro)
        {
            var novoCliente = new Cliente(request.Nome, request.Telefone, usuarioCadastro);
            ValidarCliente(novoCliente);
            if (Validar)
            {
                bool clienteExistente = repositorio.VerificarClienteExistente(request.Nome);
                if (!clienteExistente)
                {
                    return repositorio.Incluir(novoCliente);
                }
                else
                    AdicionarNotificacao("Cliente", ClienteMensagem.Cadastrado);
            }
            return null;
        }

        public Cliente Alterar(ClienteRequest request, string usuarioCadastro)
        {
            var clienteExistente = repositorio.ObterPorId(request.Id);
            if (clienteExistente != null)
            {
                clienteExistente.AlterarTelefone(request.Telefone);
                ValidarCliente(clienteExistente);
                if (Validar)
                {
                    return repositorio.Alterar(clienteExistente);
                }
            }
            else
                AdicionarNotificacao("Cliente", ClienteMensagem.NaoEncontrado);
            return null;
        }

        public void Inativar(int id, string usuario)
        {
            if (id <= 0)
                AdicionarNotificacao("Cliente", ClienteMensagem.NaoEncontrado);
            var clienteExistente = repositorio.ObterPorId(id);
            if (clienteExistente != null)
            {
                clienteExistente.Inativar(usuario);
                repositorio.Alterar(clienteExistente);
            }
            else
                AdicionarNotificacao("Cliente", ClienteMensagem.Inativar);
        }

        private void ValidarCliente(Cliente cliente)
        {
            if (!cliente.Validar)
                AdicionarNotificacao(cliente.Notificacoes);
        }
    }
}

using CashGame.Domain.Entities.Requests;
using CashGame.Domain.Entities.Views;
using CashGame.Domain.Interfaces.Services;
using CashGame.Domain.Services;
using CashGame.Infra.Data.Repositories;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashGame.Apresentation.Wpf.ViewModels
{
    public class ClienteWindowViewModel : BindableBase
    {
        IDialogCoordinator dialog;
        private readonly IClienteService clienteService;
        public DelegateCommand IncluirCommand { get; set; }
        public DelegateCommand AlterarCommand { get; set; }
        public DelegateCommand InativarCommand { get; set; }
        public DelegateCommand LimparTelaCommand { get; set; }
        public ProgressDialogController Progresso { get; set; }

        private bool _modoEdicao = false;
        public bool ModoEdicao
        {
            get { return _modoEdicao; }
            set
            {
                SetProperty(ref _modoEdicao, value);
            }
        }

        private bool _editarCliente = true;
        public bool EditarCliente
        {
            get { return _editarCliente; }
            set
            {
                SetProperty(ref _editarCliente, value);
            }
        }

        private ClienteRequest _request = new ClienteRequest();
        public ClienteRequest Request
        {
            get { return _request; }
            set
            {
                SetProperty(ref _request, value);
            }
        }

        private List<ClienteView> _listaCliente;
        public List<ClienteView> ListaCliente
        {
            get { return _listaCliente; }
            set { SetProperty(ref _listaCliente, value); }
        }

        private ClienteView _view = new ClienteView();
        public ClienteView View
        {
            get { return _view; }
            set
            {
                SetProperty(ref _view, value);
                ModoEdicao = _view?.Id > 0;
                EditarCliente = !_modoEdicao;
            }
        }

        public ClienteWindowViewModel(IDialogCoordinator dialog)
        {
            this.dialog = dialog;
            clienteService = new ClienteService(new ClienteRepository());
            IncluirCommand = new DelegateCommand(Incluir, () => !ModoEdicao).ObservesProperty(() => ModoEdicao);
            AlterarCommand = new DelegateCommand(Alterar, () => ModoEdicao).ObservesProperty(() => ModoEdicao);
            InativarCommand = new DelegateCommand(Inativar, () => ModoEdicao).ObservesProperty(() => ModoEdicao);
            LimparTelaCommand = new DelegateCommand(Limpar);
            BuscarClientes();
        }

        private async void Incluir()
        {
            Request.Nome = View.Nome;
            Request.Telefone= View.Telefone;
            var clienteCriado = clienteService.Incluir(Request, "Carlosg");
            if (!clienteService.Validar)
            {
                var linq = clienteService.Notificacoes.Select(msg => msg.Mensagem);
                await this.dialog.ShowMessageAsync(this, "Atenção", string.Join("\r\n", linq));
                clienteService.LimparNotificacoes();
            }
            if (clienteCriado != null)
            {
                Progresso = await dialog.ShowProgressAsync(this, "Progresso", "Incluindo dados do cliente. Aguarde...");
                Progresso.SetIndeterminate();
                var t = Task.Factory.StartNew(() => { BuscarClientes(); });
                await t;
                await Progresso?.CloseAsync();
                await this.dialog.ShowMessageAsync(this, "Atenção", "Cliente cadastrado com sucesso !!!");
                Limpar();
            }
        }

        private async void Alterar()
        {
            if (View != null && View.Id > 0)
            {
                var clienteExistente = clienteService.ObterPorId(View.Id);
                if (clienteExistente != null)
                {
                    Request.Id = View.Id;
                    Request.Telefone= View.Telefone;
                    clienteExistente = clienteService.Alterar(Request, "Carlosg");
                    if (clienteService.Validar)
                    {
                        Progresso = await dialog.ShowProgressAsync(this, "Progresso", "Alterando dados do cliente. Aguarde...");
                        Progresso.SetIndeterminate();
                        await Progresso?.CloseAsync();
                        await this.dialog.ShowMessageAsync(this, "Atenção", "Cliente alterado com sucesso !!!");
                        Limpar();
                    }
                    else
                    {
                        await this.dialog.ShowMessageAsync(this, "Atenção", string.Join("\r\n", clienteService.Notificacoes.Select(s => s.Mensagem)));
                        clienteService.LimparNotificacoes();
                    }
                    BuscarClientes();
                }
            }
        }

        private async void Inativar()
        {
            if (View != null && View.Id > 0)
            {
                var clienteExistente = clienteService.ObterPorId(View.Id);
                if (clienteExistente != null)
                {
                    Progresso = await dialog.ShowProgressAsync(this, "Progresso", "Inativando cliente. Aguarde...");
                    Progresso.SetIndeterminate();
                    var t = Task.Factory.StartNew(() => { clienteService.Inativar(View.Id, "Carlosg"); });
                    await t;
                    await Progresso?.CloseAsync();
                    await this.dialog.ShowMessageAsync(this, "Atenção", "Cliente inativado com sucesso !!!");
                    Limpar();
                    BuscarClientes();
                }
                else
                {
                    await this.dialog.ShowMessageAsync(this, "Atenção", string.Join("\r\n", clienteExistente.Notificacoes.Select(s => s.Mensagem)));
                    clienteService.LimparNotificacoes();
                }
            }
        }

        private void BuscarClientes()
        {
            ListaCliente = clienteService.ListarTodos().ToList();
        }

        private void Limpar()
        {
            View = new ClienteView();
        }
    }
}

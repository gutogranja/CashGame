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
    public class ComprarFichaWindowViewModel : BindableBase
    {
        IDialogCoordinator dialog;
        private readonly IComprarFichaService comprarFichaService;
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

        private bool _editarCompra = true;
        public bool EditarCompra
        {
            get { return _editarCompra; }
            set
            {
                SetProperty(ref _editarCompra, value);
            }
        }

        private ComprarFichaRequest _request = new ComprarFichaRequest();
        public ComprarFichaRequest Request
        {
            get { return _request; }
            set
            {
                SetProperty(ref _request, value);
            }
        }

        private List<ComprarFichaView> _listaCompra;
        public List<ComprarFichaView> ListaCompra
        {
            get { return _listaCompra; }
            set { SetProperty(ref _listaCompra, value); }
        }

        private ComprarFichaView _view = new ComprarFichaView();
        public ComprarFichaView View
        {
            get { return _view; }
            set
            {
                SetProperty(ref _view, value);
                ModoEdicao = _view?.Id > 0;
                EditarCompra = !_modoEdicao;
            }
        }

        private List<ClienteView> _listaCliente;
        public List<ClienteView> ListaCliente
        {
            get { return _listaCliente; }
            set { SetProperty(ref _listaCliente, value); }
        }

        public ComprarFichaWindowViewModel(IDialogCoordinator dialog)
        {
            this.dialog = dialog;
            comprarFichaService = new ComprarFichaService(new ComprarFichaRepository());
            clienteService = new ClienteService(new ClienteRepository());
            IncluirCommand = new DelegateCommand(Incluir, () => !ModoEdicao).ObservesProperty(() => ModoEdicao);
            AlterarCommand = new DelegateCommand(Alterar, () => ModoEdicao).ObservesProperty(() => ModoEdicao);
            InativarCommand = new DelegateCommand(Inativar, () => ModoEdicao).ObservesProperty(() => ModoEdicao);
            LimparTelaCommand = new DelegateCommand(Limpar);
            BuscarClientes();
            BuscarCompras();
        }

        private async void Incluir()
        {
            Request.IdCliente = View.IdCliente;
            Request.Data = View.Data;
            Request.Valor = View.Valor;
            var compraEfetuada = comprarFichaService.Incluir(Request, "Carlosg");
            if (!comprarFichaService.Validar)
            {
                var linq = comprarFichaService.Notificacoes.Select(msg => msg.Mensagem);
                await this.dialog.ShowMessageAsync(this, "Atenção", string.Join("\r\n", linq));
                comprarFichaService.LimparNotificacoes();
            }
            if (compraEfetuada != null)
            {
                Progresso = await dialog.ShowProgressAsync(this, "Progresso", "Efetuando compra de fichas. Aguarde...");
                Progresso.SetIndeterminate();
                var t = Task.Factory.StartNew(() => { BuscarCompras(); });
                await t;
                await Progresso?.CloseAsync();
                await this.dialog.ShowMessageAsync(this, "Atenção", "Compra efetuada com sucesso !!!");
                Limpar();
            }
        }

        private async void Alterar()
        {
            if (View != null && View.Id > 0)
            {
                var compraEfetuada = comprarFichaService.ObterPorId(View.Id);
                if (compraEfetuada != null)
                {
                    Request.Id = View.Id;
                    Request.IdCliente = View.IdCliente;
                    Request.Valor = View.Valor;
                    compraEfetuada = comprarFichaService.Alterar(Request, "Carlosg");
                    if (comprarFichaService.Validar)
                    {
                        Progresso = await dialog.ShowProgressAsync(this, "Progresso", "Alterando dados da compra de fichas. Aguarde...");
                        Progresso.SetIndeterminate();
                        await Progresso?.CloseAsync();
                        await this.dialog.ShowMessageAsync(this, "Atenção", "Compra de fichas alterada com sucesso !!!");
                        Limpar();
                    }
                    else
                    {
                        await this.dialog.ShowMessageAsync(this, "Atenção", string.Join("\r\n", comprarFichaService.Notificacoes.Select(s => s.Mensagem)));
                        comprarFichaService.LimparNotificacoes();
                    }
                    BuscarCompras();
                }
            }
        }

        private async void Inativar()
        {
            if (View != null && View.Id > 0)
            {
                var compraEfetuada = comprarFichaService.ObterPorId(View.Id);
                if (compraEfetuada != null)
                {
                    Progresso = await dialog.ShowProgressAsync(this, "Progresso", "Inativando a compra de fichas. Aguarde...");
                    Progresso.SetIndeterminate();
                    var t = Task.Factory.StartNew(() => { comprarFichaService.Inativar(View.Id, "Carlosg"); });
                    await t;
                    await Progresso?.CloseAsync();
                    await this.dialog.ShowMessageAsync(this, "Atenção", "Compra de fichas inativada com sucesso !!!");
                    Limpar();
                    BuscarCompras();
                }
                else
                {
                    await this.dialog.ShowMessageAsync(this, "Atenção", string.Join("\r\n", compraEfetuada.Notificacoes.Select(s => s.Mensagem)));
                    comprarFichaService.LimparNotificacoes();
                }
            }
        }

        private void BuscarClientes()
        {
            ListaCliente = clienteService.ListarTodos().ToList();
        }

        private void BuscarCompras()
        {
            ListaCompra = comprarFichaService.ListarTodos().ToList();
        }

        private void Limpar()
        {
            View = new ComprarFichaView();
        }
    }
}

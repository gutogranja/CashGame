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
    public class PagtoJogadorWindowViewModel : BindableBase
    {
        IDialogCoordinator dialog;
        private readonly IPagtoJogadorService pagtoJogadorService;
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

        private bool _editarPagto = true;
        public bool EditarPagto
        {
            get { return _editarPagto; }
            set
            {
                SetProperty(ref _editarPagto, value);
            }
        }

        private PagtoJogadorRequest _request = new PagtoJogadorRequest();
        public PagtoJogadorRequest Request
        {
            get { return _request; }
            set
            {
                SetProperty(ref _request, value);
            }
        }

        private List<PagtoJogadorView> _listaPagamento;
        public List<PagtoJogadorView> ListaPagamento
        {
            get { return _listaPagamento; }
            set { SetProperty(ref _listaPagamento, value); }
        }

        private PagtoJogadorView _view = new PagtoJogadorView();
        public PagtoJogadorView View
        {
            get { return _view; }
            set
            {
                SetProperty(ref _view, value);
                ModoEdicao = _view?.Id > 0;
                EditarPagto = !_modoEdicao;
            }
        }

        private List<ClienteView> _listaCliente;
        public List<ClienteView> ListaCliente
        {
            get { return _listaCliente; }
            set { SetProperty(ref _listaCliente, value); }
        }

        public PagtoJogadorWindowViewModel(IDialogCoordinator dialog)
        {
            this.dialog = dialog;
            pagtoJogadorService = new PagtoJogadorService(new PagtoJogadorRepository());
            clienteService = new ClienteService(new ClienteRepository());
            IncluirCommand = new DelegateCommand(Incluir, () => !ModoEdicao).ObservesProperty(() => ModoEdicao);
            AlterarCommand = new DelegateCommand(Alterar, () => ModoEdicao).ObservesProperty(() => ModoEdicao);
            InativarCommand = new DelegateCommand(Inativar, () => ModoEdicao).ObservesProperty(() => ModoEdicao);
            LimparTelaCommand = new DelegateCommand(Limpar);
            BuscarClientes();
            BuscarPagamentos();
        }

        private async void Incluir()
        {
            Request.IdCliente = View.IdCliente;
            Request.Data = View.Data;
            Request.Valor = View.Valor;
            var pagtoEfetuado = pagtoJogadorService.Incluir(Request, "Carlosg");
            if (!pagtoJogadorService.Validar)
            {
                var linq = pagtoJogadorService.Notificacoes.Select(msg => msg.Mensagem);
                await this.dialog.ShowMessageAsync(this, "Atenção", string.Join("\r\n", linq));
                pagtoJogadorService.LimparNotificacoes();
            }
            if (pagtoEfetuado != null)
            {
                Progresso = await dialog.ShowProgressAsync(this, "Progresso", "Efetuando pagamento ao jogador. Aguarde...");
                Progresso.SetIndeterminate();
                var t = Task.Factory.StartNew(() => { BuscarPagamentos(); });
                await t;
                await Progresso?.CloseAsync();
                await this.dialog.ShowMessageAsync(this, "Atenção", "Pagamento efetuado com sucesso !!!");
                Limpar();
            }
        }

        private async void Alterar()
        {
            if (View != null && View.Id > 0)
            {
                var pagtoEfetuado = pagtoJogadorService.ObterPorId(View.Id);
                if (pagtoEfetuado != null)
                {
                    Request.Id = View.Id;
                    Request.Valor = View.Valor;
                    pagtoEfetuado = pagtoJogadorService.Alterar(Request, "Carlosg");
                    if (pagtoJogadorService.Validar)
                    {
                        Progresso = await dialog.ShowProgressAsync(this, "Progresso", "Alterando dados do pagamento ao jogador. Aguarde...");
                        Progresso.SetIndeterminate();
                        await Progresso?.CloseAsync();
                        await this.dialog.ShowMessageAsync(this, "Atenção", "Pagamento ao jogador alterado com sucesso !!!");
                        Limpar();
                    }
                    else
                    {
                        await this.dialog.ShowMessageAsync(this, "Atenção", string.Join("\r\n", pagtoJogadorService.Notificacoes.Select(s => s.Mensagem)));
                        pagtoJogadorService.LimparNotificacoes();
                    }
                    BuscarPagamentos();
                }
            }
        }

        private async void Inativar()
        {
            if (View != null && View.Id > 0)
            {
                var pagtoEfetuado = pagtoJogadorService.ObterPorId(View.Id);
                if (pagtoEfetuado != null)
                {
                    Progresso = await dialog.ShowProgressAsync(this, "Progresso", "Inativando o pagamento ao jogador. Aguarde...");
                    Progresso.SetIndeterminate();
                    var t = Task.Factory.StartNew(() => { pagtoJogadorService.Inativar(View.Id, "Carlosg"); });
                    await t;
                    await Progresso?.CloseAsync();
                    await this.dialog.ShowMessageAsync(this, "Atenção", "Pagamento ao jogador inativado com sucesso !!!");
                    Limpar();
                    BuscarPagamentos();
                }
                else
                {
                    await this.dialog.ShowMessageAsync(this, "Atenção", string.Join("\r\n", pagtoEfetuado.Notificacoes.Select(s => s.Mensagem)));
                    pagtoJogadorService.LimparNotificacoes();
                }
            }
        }

        private void BuscarClientes()
        {
            ListaCliente = clienteService.ListarTodos().ToList();
        }

        private void BuscarPagamentos()
        {
            ListaPagamento = pagtoJogadorService.ListarTodos().ToList();
        }

        private void Limpar()
        {
            View = new PagtoJogadorView();
        }
    }
}

using CashGame.Domain.Entities.Requests;
using CashGame.Domain.Entities.Views;
using CashGame.Domain.Interfaces.Services;
using CashGame.Domain.Services;
using CashGame.Infra.Data.Repositories;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CashGame.Apresentation.Wpf.ViewModels
{
    public class PagtoJogadorWindowViewModel : BindableBase
    {
        IDialogCoordinator dialog;
        private readonly IPagtoJogadorService pagtoJogadorService;
        private readonly IClienteService clienteService;
        private readonly PagtoJogadorService service = new PagtoJogadorService(new PagtoJogadorRepository());
        public DelegateCommand IncluirCommand { get; set; }
        public DelegateCommand AlterarCommand { get; set; }
        public DelegateCommand InativarCommand { get; set; }
        public DelegateCommand LimparTelaCommand { get; set; }
        public DelegateCommand PesquisarCommand { get; set; }
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

        private DateTime _dataPesquisar = DateTime.Now;
        public DateTime DataPesquisar
        {
            get { return _dataPesquisar; }
            set
            {
                SetProperty(ref _dataPesquisar, value);
                if (VisibilidadeData == Visibility.Visible)
                    ListaPagamento = service.ListarPorData(DataPesquisar).ToList();
            }
        }

        private Visibility _visibilidadePesquisar = Visibility.Hidden;
        public Visibility VisibilidadePesquisar
        {
            get { return _visibilidadePesquisar; }
            set
            {
                SetProperty(ref _visibilidadePesquisar, value);
            }
        }

        private Visibility _visibilidadeData = Visibility.Hidden;
        public Visibility VisibilidadeData
        {
            get { return _visibilidadeData; }
            set
            {
                SetProperty(ref _visibilidadeData, value);
                if (_visibilidadeData == Visibility.Visible)
                    ListaPagamento = service.ListarPorData(DataPesquisar).ToList();
            }
        }

        private bool _controlaVisibilidade = false;
        public bool ControlaVisibilidade
        {
            get { return _controlaVisibilidade; }
            set
            {
                SetProperty(ref _controlaVisibilidade, value);
                VisibilidadePesquisar = _controlaVisibilidade ? Visibility.Visible : Visibility.Hidden;
            }
        }

        private List<PagtoJogadorView> _listaPagamento;
        public List<PagtoJogadorView> ListaPagamento
        {
            get { return _listaPagamento; }
            set
            {
                SetProperty(ref _listaPagamento, value);
            }
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
            set
            {
                SetProperty(ref _listaCliente, value);
            }
        }

        private ClienteView _clienteView = new ClienteView();
        public ClienteView ClienteView
        {
            get { return _clienteView; }
            set
            {
                SetProperty(ref _clienteView, value);
            }
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
            PesquisarCommand = new DelegateCommand(PesquisarPorData);
            BuscarClientes();
            BuscarPagamentos();
        }

        private async void Incluir()
        {
            Request.Id = View.Id;
            Request.IdCliente = ClienteView.Id;
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
                var t = Task.Factory.StartNew(() => { Limpar(); });
                await t;
                await Progresso?.CloseAsync();
                await this.dialog.ShowMessageAsync(this, "Atenção", "Pagamento efetuado com sucesso !!!");
                //Limpar();
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
                        BuscarPagamentos();
                    }
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
                    var inativarPagto = await MessageBoxQuestion("Atenção!", "Deseja mesmo inativar este pagamento ao jogador <S/N>?");
                    if (inativarPagto)
                    {
                        Progresso = await dialog.ShowProgressAsync(this, "Progresso", "Inativando o pagamento ao jogador. Aguarde...");
                        Progresso.SetIndeterminate();
                        var t = Task.Factory.StartNew(() => { pagtoJogadorService.Inativar(View.Id, "Carlosg"); });
                        await t;
                        await Progresso?.CloseAsync();
                        await this.dialog.ShowMessageAsync(this, "Atenção", "Pagamento ao jogador inativado com sucesso !!!");
                        Limpar();
                    }
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
            ControlaVisibilidade = ListaPagamento.Count > 0 ? true : false;
        }

        private void Limpar()
        {
            BuscarPagamentos();
            VisibilidadeData = Visibility.Hidden;
            View = new PagtoJogadorView();
        }

        private void PesquisarPorData()
        {
            if (VisibilidadeData == Visibility.Hidden)
            {
                DataPesquisar = DateTime.Now;
                VisibilidadeData = Visibility.Visible;
            }
        }

        public async Task<bool> MessageBoxQuestion(string titulo, string msg)
        {
            var configuracoes = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Sim",
                NegativeButtonText = "Não",
            };
            MessageDialogResult resultado = await this.dialog.ShowMessageAsync(this, titulo, msg, MessageDialogStyle.AffirmativeAndNegative, configuracoes);
            return (resultado == MessageDialogResult.Affirmative);
        }
    }
}

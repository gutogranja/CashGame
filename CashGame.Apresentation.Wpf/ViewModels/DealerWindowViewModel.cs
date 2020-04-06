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
    public class DealerWindowViewModel : BindableBase
    {
        IDialogCoordinator dialog;
        private readonly IDealerService dealerService;
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

        private bool _editarDealer = true;
        public bool EditarDealer
        {
            get { return _editarDealer; }
            set
            {
                SetProperty(ref _editarDealer, value);
            }
        }

        private DealerRequest _request = new DealerRequest();
        public DealerRequest Request
        {
            get { return _request; }
            set
            {
                SetProperty(ref _request, value);
            }
        }

        private List<DealerView> _listaDealer;
        public List<DealerView> ListaDealer
        {
            get { return _listaDealer; }
            set { SetProperty(ref _listaDealer, value); }
        }

        private DealerView _view = new DealerView();
        public DealerView View
        {
            get { return _view; }
            set
            {
                SetProperty(ref _view, value);
                ModoEdicao = _view?.Id > 0;
                EditarDealer = !_modoEdicao;
            }
        }

        public DealerWindowViewModel(IDialogCoordinator dialog)
        {
            this.dialog = dialog;
            dealerService = new DealerService(new DealerRepository());
            IncluirCommand = new DelegateCommand(Incluir, () => !ModoEdicao).ObservesProperty(() => ModoEdicao);
            AlterarCommand = new DelegateCommand(Alterar, () => ModoEdicao).ObservesProperty(() => ModoEdicao);
            InativarCommand = new DelegateCommand(Inativar, () => ModoEdicao).ObservesProperty(() => ModoEdicao);
            LimparTelaCommand = new DelegateCommand(Limpar);
            BuscarDealers();
        }

        private async void Incluir()
        {
            Request.Nome = View.Nome;
            Request.Telefone = View.Telefone;
            var dealerCriado = dealerService.Incluir(Request, "Carlosg");
            if (!dealerService.Validar)
            {
                var linq = dealerService.Notificacoes.Select(msg => msg.Mensagem);
                await this.dialog.ShowMessageAsync(this, "Atenção", string.Join("\r\n", linq));
                dealerService.LimparNotificacoes();
            }
            if (dealerCriado != null)
            {
                Progresso = await dialog.ShowProgressAsync(this, "Progresso", "Incluindo dados do dealer. Aguarde...");
                Progresso.SetIndeterminate();
                var t = Task.Factory.StartNew(() => { BuscarDealers(); });
                await t;
                await Progresso?.CloseAsync();
                await this.dialog.ShowMessageAsync(this, "Atenção", "Dealer cadastrado com sucesso !!!");
                Limpar();
            }
        }

        private async void Alterar()
        {
            if (View != null && View.Id > 0)
            {
                var dealerExistente = dealerService.ObterPorId(View.Id);
                if (dealerExistente != null)
                {
                    Request.Id = View.Id;
                    Request.Telefone = View.Telefone;
                    dealerExistente = dealerService.Alterar(Request, "Carlosg");
                    if (dealerService.Validar)
                    {
                        Progresso = await dialog.ShowProgressAsync(this, "Progresso", "Alterando dados do dealer. Aguarde...");
                        Progresso.SetIndeterminate();
                        await Progresso?.CloseAsync();
                        await this.dialog.ShowMessageAsync(this, "Atenção", "Dealer alterado com sucesso !!!");
                        Limpar();
                    }
                    else
                    {
                        await this.dialog.ShowMessageAsync(this, "Atenção", string.Join("\r\n", dealerService.Notificacoes.Select(s => s.Mensagem)));
                        dealerService.LimparNotificacoes();
                    }
                    BuscarDealers();
                }
            }
        }

        private async void Inativar()
        {
            if (View != null && View.Id > 0)
            {
                var dealerExistente = dealerService.ObterPorId(View.Id);
                if (dealerExistente != null)
                {
                    Progresso = await dialog.ShowProgressAsync(this, "Progresso", "Inativando dealer. Aguarde...");
                    Progresso.SetIndeterminate();
                    var t = Task.Factory.StartNew(() => { dealerService.Inativar(View.Id, "Carlosg"); });
                    await t;
                    await Progresso?.CloseAsync();
                    await this.dialog.ShowMessageAsync(this, "Atenção", "Dealer inativado com sucesso !!!");
                    Limpar();
                    BuscarDealers();
                }
                else
                {
                    await this.dialog.ShowMessageAsync(this, "Atenção", string.Join("\r\n", dealerExistente.Notificacoes.Select(s => s.Mensagem)));
                    dealerService.LimparNotificacoes();
                }
            }
        }

        private void BuscarDealers()
        {
            ListaDealer = dealerService.ListarTodos().ToList();
        }

        private void Limpar()
        {
            View = new DealerView();
        }
    }
}

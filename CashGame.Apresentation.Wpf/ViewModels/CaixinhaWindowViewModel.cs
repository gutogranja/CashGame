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
    public class CaixinhaWindowViewModel : BindableBase
    {
        IDialogCoordinator dialog;
        private readonly ICaixinhaService caixinhaService;
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

        private bool _editarCaixinha = true;
        public bool EditarCaixinha
        {
            get { return _editarCaixinha; }
            set
            {
                SetProperty(ref _editarCaixinha, value);
            }
        }

        private CaixinhaRequest _request = new CaixinhaRequest();
        public CaixinhaRequest Request
        {
            get { return _request; }
            set
            {
                SetProperty(ref _request, value);
            }
        }

        private List<CaixinhaView> _listaCaixinha;
        public List<CaixinhaView> ListaCaixinha
        {
            get { return _listaCaixinha; }
            set { SetProperty(ref _listaCaixinha, value); }
        }

        private CaixinhaView _view = new CaixinhaView();
        public CaixinhaView View
        {
            get { return _view; }
            set
            {
                SetProperty(ref _view, value);
                ModoEdicao = _view?.Id > 0;
                EditarCaixinha = !_modoEdicao;
            }
        }

        private List<DealerView> _listaDealer;
        public List<DealerView> ListaDealer
        {
            get { return _listaDealer; }
            set { SetProperty(ref _listaDealer, value); }
        }

        public CaixinhaWindowViewModel(IDialogCoordinator dialog)
        {
            this.dialog = dialog;
            caixinhaService = new CaixinhaService(new CaixinhaRepository());
            dealerService = new DealerService(new DealerRepository());
            IncluirCommand = new DelegateCommand(Incluir, () => !ModoEdicao).ObservesProperty(() => ModoEdicao);
            AlterarCommand = new DelegateCommand(Alterar, () => ModoEdicao).ObservesProperty(() => ModoEdicao);
            InativarCommand = new DelegateCommand(Inativar, () => ModoEdicao).ObservesProperty(() => ModoEdicao);
            LimparTelaCommand = new DelegateCommand(Limpar);
            BuscarDealers();
            BuscarCaixinhas();
        }

        private async void Incluir()
        {
            Request.IdDealer = View.IdDealer;
            Request.Data = View.Data;
            var caixinhaDada = caixinhaService.Incluir(Request, "Carlosg");
            if (!caixinhaService.Validar)
            {
                var linq = caixinhaService.Notificacoes.Select(msg => msg.Mensagem);
                await this.dialog.ShowMessageAsync(this, "Atenção", string.Join("\r\n", linq));
                caixinhaService.LimparNotificacoes();
            }
            if (caixinhaDada != null)
            {
                Progresso = await dialog.ShowProgressAsync(this, "Progresso", "Registrando caixinha para dealer. Aguarde...");
                Progresso.SetIndeterminate();
                var t = Task.Factory.StartNew(() => { BuscarCaixinhas(); });
                await t;
                await Progresso?.CloseAsync();
                await this.dialog.ShowMessageAsync(this, "Atenção", "Caixinha para dealer registrada com sucesso !!!");
                Limpar();
            }
        }

        private async void Alterar()
        {
            if (View != null && View.Id > 0)
            {
                var caixinhaDada = caixinhaService.ObterPorId(View.Id);
                if (caixinhaDada != null)
                {
                    Request.Id = View.Id;
                    Request.Valor = View.Valor;
                    caixinhaDada = caixinhaService.Alterar(Request, "Carlosg");
                    if (caixinhaService.Validar)
                    {
                        Progresso = await dialog.ShowProgressAsync(this, "Progresso", "Alterando registro de caixinha para dealer. Aguarde...");
                        Progresso.SetIndeterminate();
                        await Progresso?.CloseAsync();
                        await this.dialog.ShowMessageAsync(this, "Atenção", "Registro de caixinha para dealer alterado com sucesso !!!");
                        Limpar();
                    }
                    else
                    {
                        await this.dialog.ShowMessageAsync(this, "Atenção", string.Join("\r\n", caixinhaService.Notificacoes.Select(s => s.Mensagem)));
                        caixinhaService.LimparNotificacoes();
                    }
                    BuscarCaixinhas();
                }
            }
        }

        private async void Inativar()
        {
            if (View != null && View.Id > 0)
            {
                var caixinhaDada = caixinhaService.ObterPorId(View.Id);
                if (caixinhaDada != null)
                {
                    Progresso = await dialog.ShowProgressAsync(this, "Progresso", "Inativando registro de caixinha para dealer. Aguarde...");
                    Progresso.SetIndeterminate();
                    var t = Task.Factory.StartNew(() => { caixinhaService.Inativar(View.Id, "Carlosg"); });
                    await t;
                    await Progresso?.CloseAsync();
                    await this.dialog.ShowMessageAsync(this, "Atenção", "Registro de caixinha para dealer inativado com sucesso !!!");
                    Limpar();
                    BuscarCaixinhas();
                }
                else
                {
                    await this.dialog.ShowMessageAsync(this, "Atenção", string.Join("\r\n", caixinhaDada.Notificacoes.Select(s => s.Mensagem)));
                    caixinhaService.LimparNotificacoes();
                }
            }
        }

        private void BuscarDealers()
        {
            ListaDealer = dealerService.ListarTodos().ToList();
        }

        private void BuscarCaixinhas()
        {
            ListaCaixinha = caixinhaService.ListarTodos().ToList();
        }

        private void Limpar()
        {
            View = new CaixinhaView();
        }
    }
}

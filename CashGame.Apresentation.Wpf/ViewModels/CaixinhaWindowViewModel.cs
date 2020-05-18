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
    public class CaixinhaWindowViewModel : BindableBase
    {
        IDialogCoordinator dialog;
        private readonly ICaixinhaService caixinhaService;
        private readonly IDealerService dealerService;
        private readonly CaixinhaService service = new CaixinhaService(new CaixinhaRepository());
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

        private DateTime _dataPesquisar = DateTime.Now;
        public DateTime DataPesquisar
        {
            get { return _dataPesquisar; }
            set
            {
                SetProperty(ref _dataPesquisar, value);
                if (VisibilidadeData == Visibility.Visible)
                    ListaCaixinha = service.ListarPorData(DataPesquisar).ToList();
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
                    ListaCaixinha = service.ListarPorData(DataPesquisar).ToList();
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

        private List<CaixinhaView> _listaCaixinha;
        public List<CaixinhaView> ListaCaixinha
        {
            get { return _listaCaixinha; }
            set
            {
                SetProperty(ref _listaCaixinha, value);
            }
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
            set
            {
                SetProperty(ref _listaDealer, value);
            }
        }

        private DealerView _dealerView = new DealerView();
        public DealerView DealerView
        {
            get { return _dealerView; }
            set
            {
                SetProperty(ref _dealerView, value);
            }
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
            PesquisarCommand = new DelegateCommand(PesquisarPorData);
            BuscarDealers();
            BuscarCaixinhas();
        }

        private async void Incluir()
        {
            Request.Id = View.Id;
            Request.IdDealer = DealerView.Id;
            Request.Data = View.Data;
            Request.Valor = View.Valor;
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
                var t = Task.Factory.StartNew(() => { Limpar(); });
                await t;
                await Progresso?.CloseAsync();
                await this.dialog.ShowMessageAsync(this, "Atenção", "Caixinha para dealer registrada com sucesso !!!");
                //Limpar();
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
                        BuscarCaixinhas();
                    }
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
                    var inativarCaixinha = await MessageBoxQuestion("Atenção!", "Deseja mesmo inativar esta caixinha para este(a) dealer <S/N>?");
                    if (inativarCaixinha)
                    {
                        Progresso = await dialog.ShowProgressAsync(this, "Progresso", "Inativando registro de caixinha para dealer. Aguarde...");
                        Progresso.SetIndeterminate();
                        var t = Task.Factory.StartNew(() => { caixinhaService.Inativar(View.Id, "Carlosg"); });
                        await t;
                        await Progresso?.CloseAsync();
                        await this.dialog.ShowMessageAsync(this, "Atenção", "Registro de caixinha para dealer inativado com sucesso !!!");
                        Limpar();
                    }
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
            ControlaVisibilidade = ListaCaixinha.Count > 0 ? true : false;
        }

        private void Limpar()
        {
            BuscarCaixinhas();
            VisibilidadeData = Visibility.Hidden;
            View = new CaixinhaView();
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

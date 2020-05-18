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
    public class RakeWindowViewModel : BindableBase
    {
        IDialogCoordinator dialog;
        private readonly IRakeService rakeService;
        private readonly RakeService service = new RakeService(new RakeRepository());
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

        private bool _editarRake = true;
        public bool EditarRake
        {
            get { return _editarRake; }
            set
            {
                SetProperty(ref _editarRake, value);
            }
        }

        private RakeRequest _request = new RakeRequest();
        public RakeRequest Request
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
                    ListaRake = service.ListarPorData(DataPesquisar).ToList();
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
                    ListaRake = service.ListarPorData(DataPesquisar).ToList();
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

        private List<RakeView> _listaRake;
        public List<RakeView> ListaRake
        {
            get { return _listaRake; }
            set
            {
                SetProperty(ref _listaRake, value);
            }
        }

        private RakeView _view = new RakeView();
        public RakeView View
        {
            get { return _view; }
            set
            {
                SetProperty(ref _view, value);
                ModoEdicao = _view?.Id > 0;
                EditarRake = !_modoEdicao;
            }
        }

        public RakeWindowViewModel(IDialogCoordinator dialog)
        {
            this.dialog = dialog;
            rakeService = new RakeService(new RakeRepository());
            IncluirCommand = new DelegateCommand(Incluir, () => !ModoEdicao).ObservesProperty(() => ModoEdicao);
            AlterarCommand = new DelegateCommand(Alterar, () => ModoEdicao).ObservesProperty(() => ModoEdicao);
            InativarCommand = new DelegateCommand(Inativar, () => ModoEdicao).ObservesProperty(() => ModoEdicao);
            LimparTelaCommand = new DelegateCommand(Limpar);
            PesquisarCommand = new DelegateCommand(PesquisarPorData);
            BuscarRakes();
        }

        private async void Incluir()
        {
            Request.Id = View.Id;
            Request.DataRetirada = View.DataRetirada;
            Request.Valor = View.Valor;
            var rakeRetirado = rakeService.Incluir(Request, "Carlosg");
            if (!rakeService.Validar)
            {
                var linq = rakeService.Notificacoes.Select(msg => msg.Mensagem);
                await this.dialog.ShowMessageAsync(this, "Atenção", string.Join("\r\n", linq));
                rakeService.LimparNotificacoes();
            }
            if (rakeRetirado != null)
            {
                Progresso = await dialog.ShowProgressAsync(this, "Progresso", "Registrando retirada do rake. Aguarde...");
                Progresso.SetIndeterminate();
                var t = Task.Factory.StartNew(() => { Limpar(); });
                await t;
                await Progresso?.CloseAsync();
                await this.dialog.ShowMessageAsync(this, "Atenção", "Retirada de rake registrada com sucesso !!!");
                //Limpar();
            }
        }

        private async void Alterar()
        {
            if (View != null && View.Id > 0)
            {
                var retiradaEfetuada = rakeService.ObterPorId(View.Id);
                if (retiradaEfetuada != null)
                {
                    Request.Id = View.Id;
                    Request.Valor = View.Valor;
                    retiradaEfetuada = rakeService.Alterar(Request, "Carlosg");
                    if (rakeService.Validar)
                    {
                        Progresso = await dialog.ShowProgressAsync(this, "Progresso", "Alterando dados da retirada de rake. Aguarde...");
                        Progresso.SetIndeterminate();
                        await Progresso?.CloseAsync();
                        await this.dialog.ShowMessageAsync(this, "Atenção", "Retirada de rake alterada com sucesso !!!");
                        Limpar();
                    }
                    else
                    {
                        await this.dialog.ShowMessageAsync(this, "Atenção", string.Join("\r\n", rakeService.Notificacoes.Select(s => s.Mensagem)));
                        rakeService.LimparNotificacoes();
                        BuscarRakes();
                    }
                }
            }
        }

        private async void Inativar()
        {
            if (View != null && View.Id > 0)
            {
                var retiradaEfetuada = rakeService.ObterPorId(View.Id);
                if (retiradaEfetuada != null)
                {
                    var inativarRake = await MessageBoxQuestion("Atenção!", "Deseja mesmo inativar esta retirada de rake <S/N>?");
                    if (inativarRake)
                    {
                        Progresso = await dialog.ShowProgressAsync(this, "Progresso", "Inativando a retirada de rake. Aguarde...");
                        Progresso.SetIndeterminate();
                        var t = Task.Factory.StartNew(() => { rakeService.Inativar(View.Id, "Carlosg"); });
                        await t;
                        await Progresso?.CloseAsync();
                        await this.dialog.ShowMessageAsync(this, "Atenção", "Retirada de rake inativado com sucesso !!!");
                        Limpar();
                    }
                }
                else
                {
                    await this.dialog.ShowMessageAsync(this, "Atenção", string.Join("\r\n", retiradaEfetuada.Notificacoes.Select(s => s.Mensagem)));
                    rakeService.LimparNotificacoes();
                }
            }
        }

        private void BuscarRakes()
        {
            ListaRake = rakeService.ListarTodos().ToList();
            ControlaVisibilidade = ListaRake.Count > 0 ? true : false;
        }

        private void Limpar()
        {
            BuscarRakes();
            VisibilidadeData = Visibility.Hidden;
            View = new RakeView();
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

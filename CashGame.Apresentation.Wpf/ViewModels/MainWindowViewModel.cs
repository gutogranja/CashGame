using CashGame.Apresentation.Wpf.Views;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace CashGame.Apresentation.Wpf.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {

        //private IDialogCoordinator dlgCoord;
        //public IDialogCoordinator DlgCoord
        //{
        //    get
        //    {
        //        if (dlgCoord == null)
        //            dlgCoord = DialogCoordinator.Instance;
        //        return dlgCoord;
        //    }
        //    set
        //    {
        //        SetProperty(ref dlgCoord, value);
        //    }
        //}



        IDialogCoordinator dialog;
        public DelegateCommand AbrirMenuCommand { get; set; }
        public DelegateCommand FecharMenuCommand { get; set; }
        public DelegateCommand CadastrarClientesCommand { get; set; }
        public DelegateCommand CadastrarDealersCommand { get; set; }
        public DelegateCommand RegistrarCaixinhasCommand { get; set; }
        public DelegateCommand ComprarFichasCommand { get; set; }
        public DelegateCommand FechamentoCommand { get; set; }
        public DelegateCommand RakeCommand { get; set; }
        public DelegateCommand PagtoClienteCommand { get; set; }
        public DelegateCommand SairCommand { get; set; }

        private Visibility _visibilidadeAbrir = Visibility.Visible;
        public Visibility VisibilidadeAbrir
        {
            get { return _visibilidadeAbrir; }
            set
            {
                SetProperty(ref _visibilidadeAbrir, value);
            }
        }

        private Visibility _visibilidadeFechar = Visibility.Collapsed;
        public Visibility VisibilidadeFechar
        {
            get { return _visibilidadeFechar; }
            set
            {
                SetProperty(ref _visibilidadeFechar, value);
            }
        }

        public MainWindowViewModel(IDialogCoordinator dialog)
        {
            this.dialog = dialog;
            AbrirMenuCommand = new DelegateCommand(AbrirMenu);
            FecharMenuCommand = new DelegateCommand(FecharMenu);
            CadastrarClientesCommand = new DelegateCommand(CadastrarClientes);
            CadastrarDealersCommand = new DelegateCommand(CadastrarDealers);
            RegistrarCaixinhasCommand = new DelegateCommand(RegistrarCaixinha);
            ComprarFichasCommand = new DelegateCommand(ComprarFichas);
            FechamentoCommand = new DelegateCommand(Fechamento);
            RakeCommand = new DelegateCommand(RetirarRake);
            PagtoClienteCommand = new DelegateCommand(PagtoCliente);
            SairCommand = new DelegateCommand(Sair);
        }

        private void AbrirMenu()
        {
            VisibilidadeAbrir = Visibility.Collapsed;
            VisibilidadeFechar = Visibility.Visible;
        }

        private void FecharMenu()
        {
            VisibilidadeFechar = Visibility.Collapsed;
            VisibilidadeAbrir = Visibility.Visible;
        }

        private void CadastrarClientes()
        {
            ClienteWindow novaJanela = new ClienteWindow();
            novaJanela.ShowDialog();
        }

        private void CadastrarDealers()
        {
            DealerWindow novaJanela = new DealerWindow();
            novaJanela.ShowDialog();
        }

        private void RegistrarCaixinha()
        {
            CaixinhaWindow novaJanela = new CaixinhaWindow();
            novaJanela.ShowDialog();
        }

        private void ComprarFichas()
        {
            ComprarFichaWindow novaJanela = new ComprarFichaWindow();
            novaJanela.ShowDialog();
        }

        private void Fechamento()
        {
            FechamentoWindow novaJanela = new FechamentoWindow();
            novaJanela.ShowDialog();
        }

        private void RetirarRake()
        {
            RakeWindow novaJanela = new RakeWindow();
            novaJanela.ShowDialog();
        }

        private void PagtoCliente()
        {
            PagtoJogadorWindow novaJanela = new PagtoJogadorWindow();
            novaJanela.ShowDialog();
        }

        private void Sair()
        {
            Application.Current.Shutdown();
        }

        //public async Task<bool> MessageBoxQuestion(string titulo, string msg)
        //{
        //    var configuracoes = new MetroDialogSettings()
        //    {
        //        AffirmativeButtonText = "Sim",
        //        NegativeButtonText = "Não",
        //    };
        //    MessageDialogResult resultado = await DlgCoord.ShowMessageAsync(this, titulo, msg, MessageDialogStyle.AffirmativeAndNegative, configuracoes);
        //    return (resultado == MessageDialogResult.Affirmative);
        //}

    }
}

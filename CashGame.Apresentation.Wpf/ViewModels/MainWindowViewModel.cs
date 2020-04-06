using CashGame.Apresentation.Wpf.Views;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Windows;

namespace CashGame.Apresentation.Wpf.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
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

        private bool _visivel = true;
        public bool Visivel
        {
            get { return _visivel; }
            set
            {
                SetProperty(ref _visivel, value);
            }
        }

        private string _visibilidadeAbrir = "Visible";
        public string VisibilidadeAbrir
        {
            get { return _visibilidadeAbrir; }
            set
            {
                SetProperty(ref _visibilidadeAbrir, value);
            }
        }

        private string _visibilidadeFechar = "Collapsed";
        public string VisibilidadeFechar
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
            AbrirMenuCommand = new DelegateCommand(AbrirFecharMenu, () => Visivel).ObservesProperty(() => Visivel);
            FecharMenuCommand = new DelegateCommand(AbrirFecharMenu, () => !Visivel).ObservesProperty(() => Visivel);
            CadastrarClientesCommand = new DelegateCommand(CadastrarClientes);
            CadastrarDealersCommand = new DelegateCommand(CadastrarDealers);
            RegistrarCaixinhasCommand = new DelegateCommand(RegistrarCaixinha);
            ComprarFichasCommand = new DelegateCommand(ComprarFichas);
            //FechamentoCommand = new DelegateCommand(Fechamento);
            RakeCommand = new DelegateCommand(RetirarRake);
            PagtoClienteCommand = new DelegateCommand(PagtoCliente);
            SairCommand = new DelegateCommand(Sair);
        }

        private void AbrirFecharMenu()
        {
            VisibilidadeAbrir = Visivel ? "Collapsed" : "Visible";
            VisibilidadeFechar = Visivel ? "Visible" : "Collapsed";
            Visivel = !Visivel;
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

        //private void Fechamento()
        //{
        //    FechamentoWindow novaJanela = new FechamentoWindow();
        //    novaJanela.ShowDialog();
        //}

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
    }
}

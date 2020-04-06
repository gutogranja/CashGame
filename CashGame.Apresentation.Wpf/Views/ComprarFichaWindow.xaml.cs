using CashGame.Apresentation.Wpf.ViewModels;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace CashGame.Apresentation.Wpf.Views
{
    /// <summary>
    /// Lógica interna para ComprarFichaWindow.xaml
    /// </summary>
    public partial class ComprarFichaWindow : MetroWindow
    {
        public ComprarFichaWindow()
        {
            InitializeComponent();
            this.DataContext = new ClienteWindowViewModel(DialogCoordinator.Instance);
        }
    }
}

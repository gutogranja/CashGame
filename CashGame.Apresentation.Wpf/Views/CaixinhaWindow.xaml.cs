using CashGame.Apresentation.Wpf.ViewModels;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace CashGame.Apresentation.Wpf.Views
{
    /// <summary>
    /// Lógica interna para CaixinhaWindow.xaml
    /// </summary>
    public partial class CaixinhaWindow : MetroWindow
    {
        public CaixinhaWindow()
        {
            InitializeComponent();
            this.DataContext = new CaixinhaWindowViewModel(DialogCoordinator.Instance);
        }
    }
}

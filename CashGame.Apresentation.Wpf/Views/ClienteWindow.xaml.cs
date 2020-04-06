using CashGame.Apresentation.Wpf.ViewModels;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace CashGame.Apresentation.Wpf.Views
{
    /// <summary>
    /// Lógica interna para ClientesWindow.xaml
    /// </summary>
    public partial class ClienteWindow : MetroWindow
    {
        public ClienteWindow()
        {
            InitializeComponent();
            this.DataContext = new ClienteWindowViewModel(DialogCoordinator.Instance);
        }
    }
}

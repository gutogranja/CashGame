using CashGame.Apresentation.Wpf.ViewModels;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace CashGame.Apresentation.Wpf.Views
{
    /// <summary>
    /// Lógica interna para DealersWindow.xaml
    /// </summary>
    public partial class DealerWindow : MetroWindow
    {
        public DealerWindow()
        {
            InitializeComponent();
            this.DataContext = new DealerWindowViewModel(DialogCoordinator.Instance);
        }
    }
}

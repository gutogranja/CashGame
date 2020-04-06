using CashGame.Apresentation.Wpf.ViewModels;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace CashGame.Apresentation.Wpf.Views
{
    /// <summary>
    /// Lógica interna para RakeWindow.xaml
    /// </summary>
    public partial class RakeWindow : MetroWindow
    {
        public RakeWindow()
        {
            InitializeComponent();
            this.DataContext = new RakeWindowViewModel(DialogCoordinator.Instance);
        }
    }
}

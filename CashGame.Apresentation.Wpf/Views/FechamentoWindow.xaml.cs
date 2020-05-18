using CashGame.Apresentation.Wpf.ViewModels;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace CashGame.Apresentation.Wpf.Views
{
    /// <summary>
    /// Lógica interna para FechamentoWindow.xaml
    /// </summary>
    public partial class FechamentoWindow : MetroWindow
    {
        public FechamentoWindow()
        {
            InitializeComponent();
            this.DataContext = new FechamentoWindowViewModel(DialogCoordinator.Instance);
        }
    }
}

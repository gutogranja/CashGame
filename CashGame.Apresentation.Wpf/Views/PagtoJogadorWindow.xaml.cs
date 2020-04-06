using CashGame.Apresentation.Wpf.ViewModels;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace CashGame.Apresentation.Wpf.Views
{
    /// <summary>
    /// Lógica interna para PagtoDealerWindow.xaml
    /// </summary>
    public partial class PagtoJogadorWindow : MetroWindow
    {
        public PagtoJogadorWindow()
        {
            InitializeComponent();
            this.DataContext = new PagtoJogadorWindowViewModel(DialogCoordinator.Instance);
        }
    }
}

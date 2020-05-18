using CashGame.Domain.Services;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using System.Linq;
using System;
using CashGame.Infra.CrossCutting.Excel.Repositories;
using CashGame.Infra.Data.Repositories;

namespace CashGame.Apresentation.Wpf.ViewModels
{
    class FechamentoWindowViewModel : BindableBase
    {
        IDialogCoordinator dialog;
        private readonly FechamentoService fechamentoService;
        //private readonly ComprarFichaService comprarFichaService;
        //private readonly PagtoJogadorService pagtoJogadorService;
        //private readonly RakeService rakeService;
        //private readonly CaixinhaService caixinhaService;
        public DelegateCommand ExecutarCommand { get; set; }
        public DelegateCommand LimparTelaCommand { get; set; }
        public ProgressDialogController Progresso { get; set; }
        public DateTime DataInicial { get; set; } = DateTime.Now;
        public DateTime DataFinal { get; set; } = DateTime.Now;

        public FechamentoWindowViewModel(IDialogCoordinator dialog)
        {
            this.dialog = dialog;
            //comprarFichaService = new ComprarFichaService(new ComprarFichaRepository());
            //pagtoJogadorService = new PagtoJogadorService(new PagtoJogadorRepository());
            //rakeService = new RakeService(new RakeRepository());
            //caixinhaService = new CaixinhaService(new CaixinhaRepository());
            fechamentoService = new FechamentoService(new FechamentoRepository(), new ComprarFichaRepository(), new PagtoJogadorRepository(), new RakeRepository(), new CaixinhaRepository());
            ExecutarCommand = new DelegateCommand(Executar);
            LimparTelaCommand = new DelegateCommand(Limpar);
        }

        private async void Executar()
        {
            fechamentoService.Fechamento(DataInicial, DataFinal);
            if (!fechamentoService.Validar)
            {
                var linq = fechamentoService.Notificacoes.Select(msg => msg.Mensagem);
                await this.dialog.ShowMessageAsync(this, "Atenção", string.Join("\r\n", linq));
                fechamentoService.LimparNotificacoes();
            }
            else
            {
                Progresso = await dialog.ShowProgressAsync(this, "Progresso", "Gerando relatório de fechamento. Aguarde...");
                Progresso.SetIndeterminate();
                await Progresso?.CloseAsync();
                await this.dialog.ShowMessageAsync(this, "Atenção", "Fechamento executado com sucesso !!!");
                Limpar();
            }
        }

        private void Limpar()
        {
            DataInicial = DateTime.Now;
            DataFinal = DateTime.Now;
        }
    }
}

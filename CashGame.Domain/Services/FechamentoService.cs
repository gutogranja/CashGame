using CashGame.Domain.Entities.Views;
using CashGame.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace CashGame.Domain.Services
{
    public class FechamentoService : BaseService
    {
        private readonly IFechamentoRepository repositorio;
        private readonly IComprarFichaRepository comprarFichaRepositorio;
        private readonly IPagtoJogadorRepository pagtoJogadorRepositorio;
        private readonly IRakeRepository rakeRepositorio;
        private readonly ICaixinhaRepository caixinhaRepositorio;
        private List<ComprarFichaView> lstComprarFichas;
        private List<PagtoJogadorView> lstPagtoJogador;
        private List<CaixinhaView> lstCaixinha;
        private List<RakeView> lstRake;

        public FechamentoService(IFechamentoRepository repositorio, IComprarFichaRepository comprarFichaRepositorio, IPagtoJogadorRepository pagtoJogadorRepositorio, IRakeRepository rakeRepositorio, ICaixinhaRepository caixinhaRepositorio)
        {
            this.repositorio = repositorio;
            this.comprarFichaRepositorio = comprarFichaRepositorio;
            this.pagtoJogadorRepositorio = pagtoJogadorRepositorio;
            this.rakeRepositorio = rakeRepositorio;
            this.caixinhaRepositorio = caixinhaRepositorio;
        }

        public void Fechamento(DateTime inicial, DateTime final)
        {
            if (inicial == null)
                AdicionarNotificacao("Fechamento", "Data inicial não pode ser nula.");
            if (final == null)
                AdicionarNotificacao("Fechamento", "Data final não pode ser nula.");
            if (inicial > final)
                AdicionarNotificacao("Fechamento", "Data inicial não pode ser maior que data final.");
            if (Validar)
            {
                lstComprarFichas = comprarFichaRepositorio.ComprarFichaFechamento(inicial, final);
                if (lstComprarFichas.Count > 0)
                {
                    lstPagtoJogador = pagtoJogadorRepositorio.PagtoJogadorFechamento(inicial, final);
                    lstRake = rakeRepositorio.RakeFechamento(inicial, final);
                    lstCaixinha = caixinhaRepositorio.CaixinhaFechamento(inicial, final);
                    repositorio.ExecutarFechamento(lstComprarFichas, lstPagtoJogador, lstCaixinha, lstRake);
                }
                else
                    AdicionarNotificacao("Fechamento", "Não existe nenhum registro para fechamento. Verifique !");
            }
        }
    }
}

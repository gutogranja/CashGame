using System;
using System.Collections.Generic;
using System.Text;

namespace CashGame.Domain.Entities.Mensagens
{
    public static class DealerMensagem
    {
        public static string Nome = "Nome do dealer não pode ser vazio. Informar nome !";
        public static string Cadastrado = "Dealer já cadastrado. Verifique !";
        public static string NaoEncontrado = "Dealer não encontrado. Verifique !";
        public static string Inativar = "Não é possível inativar. Pois o delaer não existe.";
    }
}

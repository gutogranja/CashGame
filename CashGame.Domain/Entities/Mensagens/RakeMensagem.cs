using System;
using System.Collections.Generic;
using System.Text;

namespace CashGame.Domain.Entities.Mensagens
{
    public static class RakeMensagem
    {
        public static string DataRetirada= "Data da retirada não pode ser vazia. Verifique !";
        public static string Valor = "Valor da retirada não pode ser vazio ou zerado. Verifique !";
        public static string Retirado = "Rake já retirado. Verifique !";
        public static string NaoEncontrado = "Rake não encontrado. Verifique !";
        public static string Inativar = "Não é possível inativar. Pois o rake não existe.";
    }
}

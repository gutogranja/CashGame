using System;
using System.Collections.Generic;
using System.Text;

namespace CashGame.Domain.Entities.Mensagens
{
    public static class CaixinhaMensagem    
    {
        public static string Dealer = "Dealer não foi informado. Favor informar !";
        public static string Data = "Data não pode ser vazia. Favor informar data !";
        public static string Valor = "Valor não pode ser vazio ou zerado. Favor informar valor !";
        public static string CxDada = "Caixinha já foi dada para este dealer nesta data. Verifique !";
        public static string CxNaoDada = "Caixinha não foi dada para este dealer nesta data. Verifique !";
        public static string Inativar = "Não é possível inativar. Pois caixinha ainda não foi dada.";
    }
}

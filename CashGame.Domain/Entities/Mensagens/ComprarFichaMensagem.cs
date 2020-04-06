using System;
using System.Collections.Generic;
using System.Text;

namespace CashGame.Domain.Entities.Mensagens
{
    public static class ComprarFichaMensagem
    {
        public static string Cliente = "Cliente não foi informado. Favor informar !";
        public static string Data = "Data não pode ser vazia. Favor informar data !";
        public static string Valor = "Valor não pode ser vazio ou zerado. Favor informar valor !";
        public static string Efetuada = "Compra de fichas já efetuada. Verifique !";
        public static string NaoEncontrado = "Compra de fichas não foi encontrada. Verifique !";
        public static string Inativar = "Não é possível inativar. Pois a compra de fichas não existe.";
    }
}

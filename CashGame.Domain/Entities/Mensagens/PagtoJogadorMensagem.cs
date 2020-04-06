using System;
using System.Collections.Generic;
using System.Text;

namespace CashGame.Domain.Entities.Mensagens
{
    public static class PagtoJogadorMensagem
    {
        public static string Cliente = "Cliente não foi informado. Favor informar !";
        public static string Data = "Data não pode ser vazia. Favor informar data !";
        public static string Valor = "Valor não pode ser vazio ou zerado. Favor informar valor !";
        public static string Efetuado = "Pagamento ao jogador já efetuado. Verifique !";
        public static string NaoEncontrado = "Pagamento ao jogador não foi encontrado. Verifique !";
        public static string Inativar = "Não é possível inativar. Pois o pagamento ao jogador não existe.";
    }
}

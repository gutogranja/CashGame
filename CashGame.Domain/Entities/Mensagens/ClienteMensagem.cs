using System;
using System.Collections.Generic;
using System.Text;

namespace CashGame.Domain.Entities.Mensagens
{
    public static class ClienteMensagem
    {
        public static string Nome = "Nome do cliente não pode ser vazio. Informar nome !";
        public static string Cadastrado = "Cliente já cadastrado. Verifique !";
        public static string NaoEncontrado = "Cliente não encontrado. Verifique !";
        public static string Inativar = "Não é possível inativar. Pois o cliente não existe.";
    }
}

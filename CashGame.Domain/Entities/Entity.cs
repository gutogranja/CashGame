using CashGame.Domain.Helpers;
using System;

namespace CashGame.Domain.Entities
{
    public class Entity : NotificationService
    {
        public int Id { get; protected set; }
        public DateTime DataCadastro { get; private set; }
        public string UsuarioCadastro { get; private set; }
        public DateTime? DataExclusao { get; private set; }
        public string UsuarioExclusao { get; private set; }
        public bool Ativo { get; private set; }

        public Entity(string usuarioCadastro)
        {
            Ativo = true;
            DataCadastro = DateTime.Now;
            UsuarioCadastro = usuarioCadastro;
        }

        public void Inativar(string usuario)
        {
            Ativo = false;
            DataExclusao = DateTime.Now;
            UsuarioExclusao = usuario;
        }
    }
}

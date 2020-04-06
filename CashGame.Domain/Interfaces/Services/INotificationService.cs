using Dapper.Contrib.Extensions;
using CashGame.Domain.Helpers;
using System.Collections.Generic;

namespace CashGame.Domain.Interfaces.Services
{
    public interface INotificationService
    {
        [Write(false)]
        IReadOnlyCollection<Notification> Notificacoes { get; }

        [Write(false)]
        bool Validar { get; }
        void AdicionarNotificacao(IReadOnlyCollection<Notification> notificacoes);
        void LimparNotificacoes();
    }
}

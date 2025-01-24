using Claims.Infrastructure.Messaging.Models;
using MediatR;

namespace Claims.Infrastructure.Messaging.Handlers
{
    public class ItemCreatedHandler : INotificationHandler<ItemCreatedMessage>
    {
        private readonly Auditer _auditer;

        public ItemCreatedHandler(Auditer auditer)
        {
            _auditer = auditer ?? throw new ArgumentNullException(nameof(auditer));
        }

        public Task Handle(ItemCreatedMessage request, CancellationToken cancellationToken)
        {
            _auditer.AuditClaim(request.Id, "POST");
            return Task.CompletedTask;
        }
    }
}

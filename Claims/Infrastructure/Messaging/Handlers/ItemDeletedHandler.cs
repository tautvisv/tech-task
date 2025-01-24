using Claims.Infrastructure.Messaging.Models;
using MediatR;

namespace Claims.Infrastructure.Messaging.Handlers
{
    public class ItemDeletedHandler : INotificationHandler<ItemDeletedMessage>
    {
        private readonly Auditer _auditer;

        public ItemDeletedHandler(Auditer auditer)
        {
            _auditer = auditer ?? throw new ArgumentNullException(nameof(auditer));
        }

        public Task Handle(ItemDeletedMessage request, CancellationToken cancellationToken)
        {
            _auditer.AuditClaim(request.Id, "DELETE");
            return Task.CompletedTask;
        }
    }
}

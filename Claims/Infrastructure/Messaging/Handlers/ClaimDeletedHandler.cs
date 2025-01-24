using Claims.Infrastructure.Messaging.Models;
using MediatR;

namespace Claims.Infrastructure.Messaging.Handlers
{
    public class ClaimDeletedHandler : INotificationHandler<ClaimDeletedMessage>
    {
        private readonly Auditer _auditer;

        public ClaimDeletedHandler(Auditer auditer)
        {
            _auditer = auditer ?? throw new ArgumentNullException(nameof(auditer));
        }

        public Task Handle(ClaimDeletedMessage request, CancellationToken cancellationToken)
        {
            _auditer.AuditClaim(request.Id, HttpTypeEnum.DELETE);
            return Task.CompletedTask;
        }
    }
}

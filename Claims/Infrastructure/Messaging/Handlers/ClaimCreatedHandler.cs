using Claims.Infrastructure.Messaging.Models;
using MediatR;

namespace Claims.Infrastructure.Messaging.Handlers
{
    public class ClaimCreatedHandler : INotificationHandler<ClaimCreatedMessage>
    {
        private readonly Auditer _auditer;

        public ClaimCreatedHandler(Auditer auditer)
        {
            _auditer = auditer ?? throw new ArgumentNullException(nameof(auditer));
        }

        public Task Handle(ClaimCreatedMessage request, CancellationToken cancellationToken)
        {
            _auditer.AuditClaim(request.Id, HttpTypeEnum.POST);
            return Task.CompletedTask;
        }
    }
}

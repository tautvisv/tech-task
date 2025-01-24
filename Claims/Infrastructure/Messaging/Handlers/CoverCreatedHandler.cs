using Claims.Infrastructure.Messaging.Models;
using MediatR;

namespace Claims.Infrastructure.Messaging.Handlers
{
    public class CoverCreatedHandler : INotificationHandler<CoverCreatedMessage>
    {
        private readonly Auditer _auditer;

        public CoverCreatedHandler(Auditer auditer)
        {
            _auditer = auditer ?? throw new ArgumentNullException(nameof(auditer));
        }

        public Task Handle(CoverCreatedMessage request, CancellationToken cancellationToken)
        {
            _auditer.AuditCover(request.Id, "POST");
            return Task.CompletedTask;
        }
    }
}

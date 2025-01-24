using Claims.Infrastructure.Messaging.Models;
using MediatR;

namespace Claims.Infrastructure.Messaging.Handlers
{
    public class CoverDeletedHandler : INotificationHandler<CoverDeletedMessage>
    {
        private readonly Auditer _auditer;

        public CoverDeletedHandler(Auditer auditer)
        {
            _auditer = auditer ?? throw new ArgumentNullException(nameof(auditer));
        }

        public Task Handle(CoverDeletedMessage request, CancellationToken cancellationToken)
        {
            _auditer.AuditCover(request.Id, "DELETE");
            return Task.CompletedTask;
        }
    }
}

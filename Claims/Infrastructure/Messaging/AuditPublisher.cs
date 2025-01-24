using Claims.Application.Services;
using Claims.Infrastructure.Messaging.Models;
using MediatR;

namespace Claims.Infrastructure.Messaging
{
    public class AuditPublisher : IAuditClaimPublisher, IAuditCoverPublisher
    {
        private readonly IMediator _mediator;

        public AuditPublisher(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task PublishClaimCreatedAsync(string id)
        {
            var message = new ClaimCreatedMessage(id);
            await _mediator.Publish(message);
        }

        public async Task PublishClaimDeletedAsync(string id)
        {
            var message = new ClaimDeletedMessage(id);
            await _mediator.Publish(message);
        }

        public async Task PublishCoverCreatedAsync(string id)
        {
            var message = new ClaimCreatedMessage(id);
            await _mediator.Publish(message);
        }

        public async Task PublishCoverDeletedAsync(string id)
        {
            var message = new ClaimDeletedMessage(id);
            await _mediator.Publish(message);
        }
    }
}

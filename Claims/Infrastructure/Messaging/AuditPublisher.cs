using Claims.Domain.Services;
using Claims.Infrastructure.Messaging.Models;
using MediatR;

namespace Claims.Infrastructure.Messaging
{
    public class AuditPublisher : IAuditPublisher
    {
        private readonly IMediator _mediator;

        public AuditPublisher(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task PublishClaimCreatedAsync(string id)
        {
            var message = new ItemCreatedMessage(id);
            await _mediator.Publish(message, CancellationToken.None);
        }

        public async Task PublishClaimDeletedAsync(string id)
        {
            var message = new ItemDeletedMessage(id);
            await _mediator.Publish(message, CancellationToken.None);
        }
    }
}

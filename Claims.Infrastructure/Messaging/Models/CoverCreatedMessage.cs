using MediatR;

namespace Claims.Infrastructure.Messaging.Models
{
    public class CoverCreatedMessage: INotification
    {
        public string Id { get; }

        public CoverCreatedMessage(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException($"'{nameof(id)}' cannot be null or empty.", nameof(id));
            }

            Id = id;
        }
    }
}

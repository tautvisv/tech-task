using MediatR;

namespace Claims.Infrastructure.Messaging.Models
{
    public class ItemCreatedMessage: INotification
    {
        public string Id { get; }

        public ItemCreatedMessage(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException($"'{nameof(id)}' cannot be null or empty.", nameof(id));
            }

            Id = id;
        }
    }
}

namespace Claims.Domain.Exceptions
{
    public class EntityNotFoundException : DomainException
    {
        public string EntityId { get; }

        public EntityNotFoundException(string message, string entityId) : base(message)
        {
            EntityId = entityId;
        }
    }
}

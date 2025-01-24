namespace Claims.Domain.Exceptions
{
    public class DomainException: Exception
    {
        public DomainException(string message): base(message)
        {
        }
    }
    public class EntityNotFoundException : DomainException
    {
        public string EntityId { get; }

        public EntityNotFoundException(string message, string entityId) : base(message)
        {
            EntityId = entityId;
        }
    }
}

namespace Claims.Domain.Services
{
    public class AuditPublisher : IAuditPublisher
    {
        public Task PublishCreatedAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task PublishDeletedAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}

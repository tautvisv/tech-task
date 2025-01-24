namespace Claims.Domain.Services
{
    public interface IAuditPublisher
    {
        Task PublishCreatedAsync(string id);
        Task PublishDeletedAsync(string id);
    }
}

namespace Claims.Domain.Services
{
    public interface IAuditPublisher
    {
        Task PublishClaimCreatedAsync(string id);
        Task PublishClaimDeletedAsync(string id);
    }
}

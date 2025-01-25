namespace Claims.Application.Services
{
    public interface IAuditClaimPublisher
    {
        Task PublishClaimCreatedAsync(string id);
        Task PublishClaimDeletedAsync(string id);
    }
}

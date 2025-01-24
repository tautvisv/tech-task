namespace Claims.Application.Services
{
    public interface IAuditCoverPublisher
    {
        Task PublishCoverCreatedAsync(string id);
        Task PublishCoverDeletedAsync(string id);
    }
}

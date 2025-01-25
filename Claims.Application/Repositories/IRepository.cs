namespace Claims.Application.Repositories
{
    public interface IRepository<TDomain> where TDomain : class
    {
        Task<IEnumerable<TDomain>> GetAllAsync();
        Task<TDomain> GetByIdAsync(string id);
        Task CreateAsync(TDomain entity);
        Task DeleteAsync(string id);
    }
}

namespace Claims.Application.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(string id);
        Task CreateAsync(TEntity entity);
        Task DeleteAsync(string id);
    }
}

using Claims.Controllers;
using Claims.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Claims.Domain.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(string id);
        Task CreateAsync(TEntity entity);
        Task DeleteAsync(string id);
    }
    public class ClaimsRepository : IRepository<Claim>
    {
        private readonly ClaimsContext _claimsContext;
        private readonly DbSet<Claim> _claims;

        public ClaimsRepository(ClaimsContext claimsContext)
        {
            _claimsContext = claimsContext ?? throw new ArgumentNullException(nameof(claimsContext));
            _claims = _claimsContext.Claims;
        }
        public async Task CreateAsync(Claim entity)
        {
            _claims.Add(entity);
            await _claimsContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var claim = await _claims
                .Where(claim => claim.Id == id)
                .SingleOrDefaultAsync();

            if (claim is not null)
            {
                _claims.Remove(claim);
                _claimsContext.SaveChanges();
            }
        }

        public async Task<IEnumerable<Claim>> GetAllAsync()
        {
            IEnumerable<Claim> claims = await _claims.ToListAsync();
            return claims;
        }

        public async Task<Claim> GetByIdAsync(string id)
        {
            var claim = await _claims
                .Where(claim => claim.Id == id)
                .SingleOrDefaultAsync();
            return claim;
        }
    }

    public class CoversRepository : IRepository<Cover>
    {
        private readonly ClaimsContext _claimsContext;
        private readonly DbSet<Cover> _covers;

        public CoversRepository(ClaimsContext claimsContext)
        {
            _claimsContext = claimsContext ?? throw new ArgumentNullException(nameof(claimsContext));
            _covers = _claimsContext.Covers;
        }
        public async Task CreateAsync(Cover entity)
        {
            _covers.Add(entity);
            await _claimsContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var cover = await _claimsContext.Covers.Where(cover => cover.Id == id).SingleOrDefaultAsync();
            if (cover is not null)
            {
                _covers.Remove(cover);
                await _claimsContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Cover>> GetAllAsync()
        {
            var covers = await _covers.ToListAsync();
            return covers;
        }

        public async Task<Cover> GetByIdAsync(string id)
        {
            var cover = await _covers.SingleOrDefaultAsync(cover => cover.Id == id);
            return cover;
        }
    }
}

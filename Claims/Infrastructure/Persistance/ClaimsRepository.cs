using Claims.Domain.Models;
using Claims.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Claims.Infrastructure.Persistance
{
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
}

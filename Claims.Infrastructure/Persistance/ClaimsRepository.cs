using AutoMapper;
using Claims.Application.Repositories;
using Claims.Domain.Exceptions;
using Claims.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Claims.Infrastructure.Persistance
{
    public class ClaimsRepository : IRepository<Claim>
    {
        // If application is big enough then we should use database models when saving.
        // Right now it is sufficient to use domain models for persistance.
        private readonly ClaimsContext _claimsContext;
        private readonly DbSet<ClaimEntity> _claims;
        private readonly IMapper _mapper;

        public ClaimsRepository(ClaimsContext claimsContext, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _claimsContext = claimsContext ?? throw new ArgumentNullException(nameof(claimsContext));
            _claims = _claimsContext.Claims;
        }

        public async Task CreateAsync(Claim domainEntity)
        {
            var entity = _mapper.Map<Claim, ClaimEntity>(domainEntity);
            _claims.Add(entity);
            await _claimsContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var claim = await _claims
                .Where(claim => claim.Id == id)
                .SingleOrDefaultAsync();
            if (claim is null)
            {
                throw new EntityNotFoundException("Claim not found", id);
            }

            _claims.Remove(claim);
            _claimsContext.SaveChanges();
        }

        public async Task<IEnumerable<Claim>> GetAllAsync()
        {
            IEnumerable<ClaimEntity> entities = await _claims.ToListAsync();
            var claims = _mapper.Map<IEnumerable<ClaimEntity>, IEnumerable<Claim>>(entities);
            return claims;
        }

        public async Task<Claim> GetByIdAsync(string id)
        {
            var entity = await _claims
                .Where(claim => claim.Id == id)
                .SingleOrDefaultAsync();
            if (entity is null)
            {
                throw new EntityNotFoundException("Claim not found", id);
            }
            var claim = _mapper.Map<ClaimEntity, Claim>(entity);
            return claim;
        }
    }
}

using Claims.Application.Repositories;
using Claims.Domain.Exceptions;
using Claims.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Claims.Infrastructure.Persistance
{
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
            if (cover is null)
            {
                throw new EntityNotFoundException("Cover not found", id);
            }
            _covers.Remove(cover);
            await _claimsContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Cover>> GetAllAsync()
        {
            var covers = await _covers.ToListAsync();
            return covers;
        }

        public async Task<Cover> GetByIdAsync(string id)
        {
            var cover = await _covers.SingleOrDefaultAsync(cover => cover.Id == id); if (cover is null)
            {
                throw new EntityNotFoundException("Cover not found", id);
            }
            return cover;
        }
    }
}

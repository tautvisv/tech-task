using AutoMapper;
using Claims.Application.Repositories;
using Claims.Domain.Exceptions;
using Claims.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Claims.Infrastructure.Persistance
{
    public class CoversRepository : IRepository<Cover>
    {
        private readonly ClaimsContext _claimsContext;
        private readonly DbSet<CoverEntity> _covers;
        private readonly IMapper _mapper;

        // It is possible to have generic repository, but in majority cases it is easier to maintain non-generic repository.
        public CoversRepository(ClaimsContext claimsContext, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _claimsContext = claimsContext ?? throw new ArgumentNullException(nameof(claimsContext));
            _covers = _claimsContext.Covers;
        }
        public async Task CreateAsync(Cover domainEntity)
        {
            var entity = _mapper.Map<Cover, CoverEntity>(domainEntity);
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
            var entities = await _covers.ToListAsync();
            var covers = _mapper.Map<IEnumerable<CoverEntity>, IEnumerable<Cover>>(entities);
            return covers;
        }

        public async Task<Cover> GetByIdAsync(string id)
        {
            var entity = await _covers.SingleOrDefaultAsync(cover => cover.Id == id);
            if (entity is null)
            {
                throw new EntityNotFoundException("Cover not found", id);
            }
            var cover = _mapper.Map<CoverEntity, Cover>(entity);
            return cover;
        }
    }
}

using Claims.Auditing;
using Claims.Controllers;
using Claims.Domain.Models;
using Claims.Domain.Repositories;

namespace Claims.Domain.Services
{
    public class CoverService :ICoverService
    {
        private readonly Auditer _auditer;
        private readonly IPremiumService _premiumService;
        private readonly IRepository<Cover> _coverRepository;

        public CoverService(ClaimsContext claimsContext, Auditer auditer, IPremiumService premiumService, IRepository<Cover> coverRepository)
        {
            _auditer = auditer ?? throw new ArgumentNullException(nameof(auditer));
            _premiumService = premiumService ?? throw new ArgumentNullException(nameof(premiumService));
            _coverRepository = coverRepository;
        }

        public async Task<Cover> GetCoverAsync(string id)
        {
            var cover = await _coverRepository.GetByIdAsync(id);
            return cover;
        }

        public async Task<IEnumerable<Cover>> GetCoversAsync()
        {
            var covers = await _coverRepository.GetAllAsync();
            return covers;
        }

        public async Task<Cover> CreateCoverAsync(NewCover newCover)
        {
            var id = Guid.NewGuid().ToString();
            var premium = await _premiumService.ComputePremiumAsync(newCover.StartDate, newCover.EndDate, newCover.Type);
            var cover = new Cover()
            {
                Id = id,
                Premium = premium,
                EndDate = newCover.EndDate,
                StartDate = newCover.StartDate,
                Type = newCover.Type
            };
            await _coverRepository.CreateAsync(cover);
            _auditer.AuditCover(cover.Id, "POST");
            return cover;
        }

        public async Task DeleteCoverAsync(string id)
        {
            await _coverRepository.DeleteAsync(id);
            _auditer.AuditCover(id, "DELETE");
        }
    }
}
